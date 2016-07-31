using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using Commands;
namespace DesktopWindowsApp
{
    public partial class Form1 : Form
    {
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl1;
        //private ESRI.ArcGIS.Controls.AxMapControl axMapControl2;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private IToolbarMenu m_ToolbarMenu = new ToolbarMenu(); 

        //在MapControl上绘制几何图形所需成员
        private IEnvelope m_Envelope; //The envelope drawn on the MapControl
        private Object m_FillSymbol; //The symbol used to draw the envelope on the MapControl    
        private ITransformEvents_VisibleBoundsUpdatedEventHandler visBoundsUpdatedE; //The PageLayoutControl's focus map events

        //bool isMouseDown = true;
        IPoint startPoint = new ESRI.ArcGIS.Geometry.Point();
        IPoint tempPoint = new ESRI.ArcGIS.Geometry.Point();
        IPoint endPoint = new ESRI.ArcGIS.Geometry.Point();
                       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axPageLayoutControl1 = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            //this.axMapControl2 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.axMapControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            // 
            // axPageLayoutControl1
            // 
            this.axPageLayoutControl1.Location = new System.Drawing.Point(266, 56);
            this.axPageLayoutControl1.Name = "axPageLayoutControl1";
            this.axPageLayoutControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl1.OcxState")));
            this.axPageLayoutControl1.Size = new System.Drawing.Size(525, 525);
            this.axPageLayoutControl1.TabIndex = 1;
            this.axPageLayoutControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IPageLayoutControlEvents_Ax_OnMouseDownEventHandler(this.axPageLayoutControl1_OnMouseDown);
           this.axPageLayoutControl1.OnPageLayoutReplaced += new ESRI.ArcGIS.Controls.IPageLayoutControlEvents_Ax_OnPageLayoutReplacedEventHandler(this.axPageLayoutControl1_OnPageLayoutReplaced);
            // 
            // axMapControl1
            //      
            this.axMapControl1.Location = new System.Drawing.Point(0, 319);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(200, 200);
            this.axMapControl1.TabIndex = 2;

            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            this.axMapControl1.OnMouseUp += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseUpEventHandler(this.axMapControl1_OnMouseUp);
            this.axMapControl1.OnAfterDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterDrawEventHandler(this.axMapControl1_OnAfterDraw); 
            //
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 28);
            this.axToolbarControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(2130, 928);
            this.axToolbarControl1.TabIndex = 3;
            // 

            
            // axTOCControl1
            // 
            this.axTOCControl1.Location = new System.Drawing.Point(0, 56);
            this.axTOCControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(200, 200);
            this.axTOCControl1.TabIndex = 4;
            //this.axTOCControl1.OnEndLabelEdit += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnEndLabelEditEventHandler(this.axTOCControl1_OnEndLabelEdit);

            this.Controls.Add(this.axPageLayoutControl1);
            //this.Controls.Add(this.axMapControl2);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.axToolbarControl1);
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.axMapControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
     
            string progID;

           

            //Add generic commands
            progID = "esriControlToolsGeneric.ControlsOpenDocCommand";

            axToolbarControl1.AddItem(progID, -1, -1, false, 0,esriCommandStyles.esriCommandStyleIconOnly);
            //Add PageLayout navigation commands
            progID = "esriControlToolsPageLayout.ControlsPageZoomInTool"; 
            axToolbarControl1.AddItem(progID, -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconOnly);
            progID = "esriControlToolsPageLayout.ControlsPageZoomOutTool";
            axToolbarControl1.AddItem(progID, -1, -1, false, 0,esriCommandStyles.esriCommandStyleIconOnly);
            progID = "esriControlToolsPageLayout.ControlsPagePanTool";
            axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            progID = "esriControlToolsPageLayout.ControlsPageZoomWholePageCommand";
            axToolbarControl1.AddItem(progID, -1, -1, false, 0,esriCommandStyles.esriCommandStyleIconOnly);
            progID ="esriControlToolsPageLayout.ControlsPageZoomPageToLastExtentBackCommand";
            axToolbarControl1.AddItem(progID, -1, -1, false, 0,esriCommandStyles.esriCommandStyleIconOnly);
            progID ="esriControlToolsPageLayout.ControlsPageZoomPageToLastExtentForwardCommand";
            axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);

            //Add Map naviagtion commands
            progID = "esriControlToolsMapNavigation.ControlsMapZoomInTool";
            axToolbarControl1.AddItem(progID, -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconOnly);
            progID = "esriControlToolsMapNavigation.ControlsMapZoomOutTool"; 
            axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            progID = "esriControlToolsMapNavigation.ControlsMapPanTool";
            axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);
            progID = "esriControlToolsMapNavigation.ControlsMapFullExtentCommand";
            axToolbarControl1.AddItem(progID, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);


            progID = "Commands.AddDate";
            axToolbarControl1.AddItem(progID, -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconOnly);
            //Load a pre-authored
            m_ToolbarMenu.CommandPool = axToolbarControl1.CommandPool;
            //Add commands to the ToolbarMenu
            progID = "esriControlToolsPageLayout.ControlsPageZoomInFixedCommand";
            m_ToolbarMenu.AddItem(progID, -1, -1, false,
            esriCommandStyles.esriCommandStyleIconAndText);
            progID = "esriControlToolsPageLayout.ControlsPageZoomOutFixedCommand";
            m_ToolbarMenu.AddItem(progID, -1, -1, false,
            esriCommandStyles.esriCommandStyleIconAndText);
            progID = "esriControlToolsPageLayout.ControlsPageZoomWholePageCommand";
            m_ToolbarMenu.AddItem(progID, -1, -1, false,
            esriCommandStyles.esriCommandStyleIconAndText);
            progID =
            "esriControlToolsPageLayout.ControlsPageZoomPageToLastExtentBackCommand";
            m_ToolbarMenu.AddItem(progID, -1, -1, true,
            esriCommandStyles.esriCommandStyleIconAndText);
            progID =
            "esriControlToolsPageLayout.ControlsPageZoomPageToLastExtentForwardCommand";
            m_ToolbarMenu.AddItem(progID, -1, -1, false,
            esriCommandStyles.esriCommandStyleIconAndText);
            //Set the hook to the PageLayoutControl
            m_ToolbarMenu.SetHook(axPageLayoutControl1);
            //Load a pre-authored
            //Set buddy controls.
            //load map doc
            #endregion


            string filename = Application.StartupPath+ @"\data\state.mxd";
            if (axPageLayoutControl1.CheckMxFile(filename))
            {
                axPageLayoutControl1.LoadMxFile(filename, "");
            }
  

            axTOCControl1.SetBuddyControl(axPageLayoutControl1);
            axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
            CreateOverviewSymbol();
        }

        private void axPageLayoutControl1_OnPageLayoutReplaced(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnPageLayoutReplacedEvent e)
        {

            //Get the IActiveView of the focus map in the PageLayoutControl
            IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;

            //Trap the ITranformEvents of the PageLayoutCntrol's focus map
            visBoundsUpdatedE = new ITransformEvents_VisibleBoundsUpdatedEventHandler(OnVisibleBoundsUpdated);

            ((ITransformEvents_Event)activeView.ScreenDisplay.DisplayTransformation).VisibleBoundsUpdated += visBoundsUpdatedE;
            
            //Get the extent of the focus map
            m_Envelope = activeView.Extent;
            //Load the same pre-authored map document into the MapControl
            axMapControl1.LoadMxFile(axPageLayoutControl1.DocumentFilename, null,
            null);
            //Set the extent of the MapControl to the full extent of the data
            axMapControl1.Extent = axMapControl1.FullExtent;
        }
        private void CreateOverviewSymbol()
        {
            //Get the IRGBColor interface
            IRgbColor color = new RgbColor();
            //Set the color properties
            color.RGB = 255;
            //Get the ILine symbol interface
            ILineSymbol outline = new SimpleLineSymbol();
            //Set the line symbol properties
            outline.Width = 1.5;
            outline.Color = color;
            //Get the IFillSymbol interface
            ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
            //Set the fill symbol properties
            simpleFillSymbol.Outline = outline;
            simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSHollow;
            m_FillSymbol = simpleFillSymbol;
        }
        private void OnVisibleBoundsUpdated(IDisplayTransformation sender, bool sizeChanged)
        {
          
            m_Envelope = sender.VisibleBounds;
        
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.
            esriViewForeground, null, null);
        }
        private void axMapControl1_OnAfterDraw(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnAfterDrawEvent e)
        {
            if (m_Envelope == null)
            {
                return;
            }
          
            esriViewDrawPhase viewDrawPhase = (esriViewDrawPhase)e.viewDrawPhase;
            if (viewDrawPhase == esriViewDrawPhase.esriViewForeground)
            {
                IGeometry geometry = m_Envelope;
                axMapControl1.DrawShape(geometry, ref m_FillSymbol);
            }
           
        }
  
        private void axPageLayoutControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnMouseDownEvent e)

        {
           
            if (e.button == 2)
            {
                m_ToolbarMenu.PopupMenu(e.x, e.y, axPageLayoutControl1.hWnd);
            }
        }


        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (axMapControl1.Map.LayerCount > 0)
            {
                if (e.button == 1)
                {
                    //isMouseDown = true;
                    try
                    {
                        IPoint naviPoint = new ESRI.ArcGIS.Geometry.Point();
                        naviPoint.PutCoords(e.mapX, e.mapY);
                        m_Envelope.CenterAt(naviPoint);

                        IActiveView pAcv = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                        pAcv.Extent = m_Envelope;
                        //IDisplayTransformation displayTransformation = pAcv.ScreenDisplay.DisplayTransformation;
                        //displayTransformation.VisibleBounds = m_Envelope;

                        this.axMapControl1.Extent = axMapControl1.FullExtent;
                        this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                    }
                    catch { }
                }
                if (e.button == 2)
                {
                    IEnvelope en = this.axMapControl1.TrackRectangle();
                    //m_Envelope = this.axMapControl1.TrackRectangle();
                    //this.axMapControl1.Extent = m_Envelope;
                    //IPoint naviPoint = new ESRI.ArcGIS.Geometry.Point();
                    //this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                    //naviPoint
                   // startPoint.PutCoords(e.mapX, e.mapY);
                    IActiveView acv = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                    acv.Extent = en;
                    axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography,null,null);
                    m_Envelope = en;
                    this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
            }

        }

        private void axMapControl1_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            if (e.button == 1)
            {
                //isMouseDown = false;
            }
            //if (e.button == 2)
            //{
            //    endPoint.PutCoords(e.mapX, e.mapY);
            //    IEnvelope pEnvelop = new EnvelopeClass();
            //    pEnvelop.PutCoords(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            //    if (pEnvelop == null)
            //    {
            //        return;
            //    }

            //    IGeometry geometry = pEnvelop;
            //    axMapControl1.DrawShape(geometry, ref m_FillSymbol);

            //    IActiveView pAcv = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
            //    IDisplayTransformation displayTransformation = pAcv.ScreenDisplay.DisplayTransformation;
            //    displayTransformation.VisibleBounds = pEnvelop;
            //    axPageLayoutControl1.ActiveView.Refresh();
            //}
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (e.button == 1)
            {
                //if (isMouseDown)
                {
                    try
                    {
                        IPoint naviPoint = new ESRI.ArcGIS.Geometry.Point();
                        naviPoint.PutCoords(e.mapX, e.mapY);
                        m_Envelope.CenterAt(naviPoint);

                        IActiveView pAcv = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                        //IDisplayTransformation displayTransformation = pAcv.ScreenDisplay.DisplayTransformation;
                        //displayTransformation.VisibleBounds = m_Envelope;
                        pAcv.Extent = m_Envelope;

                        this.axMapControl1.Extent = axMapControl1.FullExtent;
                        this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                    }
                    catch { }
                }
            }
           

        }



    }
}