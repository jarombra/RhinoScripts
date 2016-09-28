from Rhino import *
from Rhino.Commands import *
from Rhino.Geometry import *
from Rhino.Input.Custom import *
from scriptcontext import doc

def GetOutsideObjects():
    removeInside = True
    keepOutside = True

    res, objrefs = Input.RhinoGet.GetMultipleObjects("Get output objects", False, DocObjects.ObjectType.Brep)

    if res <> Result.Success:
        return Result.Failure

    brepList = []
    gList = []

    for ob in objrefs:
        brepList.append(ob.Brep())
        gList.append(ob.ObjectId)

    boolUnions = Brep.CreateBooleanUnion(brepList, doc.ModelAbsoluteTolerance * 5)
    if not boolUnions:
        return Result.Failure

    doc.Objects.UnselectAll()

    outsideList = []
    for i in range(brepList.Count):
        succes, intCrvs, intPts = Intersect.Intersection.BrepBrep(boolUnions[0], brepList[i], doc.ModelAbsoluteTolerance * 5)
        if succes == False:
            continue

        if intCrvs.Count == 0 and intPts.Count == 0:
            if removeInside:
                doc.Objects.Delete(gList[i], True)
            continue

        if keepOutside:
            outsideList.append(brepList[i])
        else:
            doc.Objects.Select(gList[i])

    if keepOutside:
        endBreps = []

        for br in outsideList:
            goodFaces = []

            for fBr in br.Faces:
                centerBr = fBr.GetBoundingBox(True).Center

                for f in boolUnions[0].Faces:
                    center = f.GetBoundingBox(True).Center
                    if (center == centerBr):
                        goodFaces.append(f.DuplicateFace(True))

            if goodFaces.Count == 1:
                endBreps.append(goodFaces[0])
            elif goodFaces.Count > 1:
                jBrep = Brep.JoinBreps(goodFaces, doc.ModelAbsoluteTolerance * 5)
                endBreps.append(jBrep[0])

        for br in endBreps:
            doc.Objects.AddBrep(br)

        for g in gList:
            doc.Objects.Delete(g, True)

    doc.Views.Redraw()


if __name__ == "__main__":
    GetOutsideObjects()
