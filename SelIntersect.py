import rhinoscriptsyntax as rs


def SelectLayers():
    filter_layer = rs.GetLayer(title = 'select layer to filter by intersection')
    if not filter_layer: return None,None
    
    intersect_layer = rs.GetLayer(title = 'select layer with intersecting geometry')
    if not intersect_layer: return None,None
    
    return filter_layer,intersect_layer

def TestIntersection(obj,tester):
    # there are a few possible intersection methods in rhinoscriptsyntax
    # as an initial setup only a few intersections are handled
    # to make this script faster and more robuste best would be to intersect not with rhinoscriptsyntax
    # But use RhinoCommon methods instead.
    #
    #for now only rhinoscriptsyntax methods are used as an example below:
    
    # if both are breps ( (poly)surface or extrusion
    if rs.IsBrep(obj) and rs.IsBrep(tester):
        intersections = rs.IntersectBreps(obj,tester)
        if intersections : 
            #Delete intersections if they were made
            rs.DeleteObjects(intersections)
            return True

    if rs.IsMesh(obj) and rs.IsMesh(tester):
        intersections = rs.MeshMeshIntersection(obj,tester)
        if intersections : 
            #This method does not create a curve but returns a list of points
            #so nothing to delete
            return True
            
    #Mixed input needs to be handled different as is with curves.
    #if either is a mesh the other needs to be converted to a mesh as well
    
    #catchall return False
    return False
    
    
def Main():
    
    # run method to select 2 layers
    filter_layer,intersect_layer =  SelectLayers()
    if not filter_layer: return 
    
    # Get all objects on the layers (note that also hidden/locked are used)
    filter_objs = rs.ObjectsByLayer(filter_layer)
    intersect_objs  = rs.ObjectsByLayer(intersect_layer)
    
    # test if object on filter_layer intersects with any object on intersect layer
    intersecting_objs = []
    for obj in filter_objs:
        for tester in intersect_objs:
            # here the pseudo code is: does obj intersect with tester
            # I do not know the type of objects possible so
            # a more elaborate intersection methos is needed.
            if TestIntersection(obj,tester):
                intersecting_objs.append(obj)
                break #break out of for loop to prevent unneeded tests         
            
            
    if intersecting_objs:
        rs.SelectObjects(intersecting_objs)
    
Main()