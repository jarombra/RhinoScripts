using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;
using Eto.Drawing;
using Eto.Forms;

namespace HellYes
{
    public class HellYesCommand : Rhino.Commands.Command
    {
        public override string EnglishName
        {
            get { return "HellYesCommand"; }
        }

        protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
        {
            // TODO: start here modifying the behaviour of your command.
            // ---
            /*
            RhinoApp.WriteLine("The {0} command will add a line right now.", EnglishName);

            Point3d pt0;
            using (GetPoint getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the start point");
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No start point was selected.");
                    return getPointAction.CommandResult();
                }
                pt0 = getPointAction.Point();
            }

            Point3d pt1;
            using (GetPoint getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the end point");
                getPointAction.SetBasePoint(pt0, true);
                getPointAction.DrawLineFromPoint(pt0, true);
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No end point was selected.");
                    return getPointAction.CommandResult();
                }
                pt1 = getPointAction.Point();
            }

            doc.Objects.AddLine(pt0, pt1);
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command added one line to the document.", EnglishName);
            */

            var font_index = doc.Fonts.FindOrCreate("Times New Roman", true, false);

            var text_entity = new TextEntity
            {
                FontIndex = font_index,
                Justification = TextJustification.None,
                Plane = Plane.WorldXY,
                Text = "Hell Yes!",
                TextHeight = 5.0
            };

            var curves = text_entity.Explode();

            var dir = new Vector3d(0, 0, 5);
            foreach (var curve in curves)
            {
                var surface = Surface.CreateExtrusion(curve, dir);
                doc.Objects.AddSurface(surface);
            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}
