using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingUI_5_2
{
    internal class WallsUtils
    {
        public static List<WallType> GetPipingSystems(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument; 
            Document doc = uidoc.Document; 

            List<WallType> wallsSystemTypes = new FilteredElementCollector(doc) 
                .OfClass(typeof(WallType))
                .Cast<WallType>() 
                .ToList();

            return wallsSystemTypes;
        }
    }
}
