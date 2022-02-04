using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPITraningLibrary_5_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingUI_5_2
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        public DelegateCommand SelectCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public List<Element> PickedObjects { get; } = new List<Element>();
        public List<WallType> WallSystems { get; } = new List<WallType>();
        public WallType SelectedWallSystem { get; set; }


        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData=commandData;
            SaveCommand = new DelegateCommand(OnSaveCommand);
            PickedObjects = SelectionsUtils.PickObjects(commandData);
            WallSystems = WallsUtils.GetPipingSystems(commandData);
        }
        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveCommand()
        {
            RaiseHideRequest();
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (PickedObjects.Count == 0 || SelectedWallSystem == null)
                return;

            using (var ts = new Transaction(doc, "Set system type"))
            {
                ts.Start();
                foreach (var pickedObject in PickedObjects)
                {
                    if (pickedObject is Wall)
                    {
                        var owall = pickedObject as Wall;
                        var owalltype = owall.GetTypeId();
                        owall.ChangeTypeId(pickedObject.GetTypeId());
                    }
                }
                ts.Commit();
            }

            RaiseShowRequest();
        }
    }
}

