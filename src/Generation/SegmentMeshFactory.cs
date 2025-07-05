using System;
using PetriDish.Genome;
using Godot;
namespace PetriDish.Generation;

public static class SegmentMeshFactory
{
    private static int _verticesPerRing = 16;
    private static int _rings_per_segment = 9;


    private static float[] CalculateRingZPositions(SegmentGenome genome, float bulgeRadius)
    {
        
        var startToBulgeDistance = genome.Length * float.Sqrt(bulgeRadius *  bulgeRadius - genome.StartRadius * genome.StartRadius)
                                   / (float.Sqrt(bulgeRadius *  bulgeRadius - genome.StartRadius * genome.StartRadius) 
                                      +  float.Sqrt(bulgeRadius *  bulgeRadius - genome.EndRadius * genome.EndRadius)); 
        
        var endToBulgeDistance = genome.Length - startToBulgeDistance;
        
        var bulgeZPosition = genome.Length / 2 - startToBulgeDistance;
        
        var zPositionPerRing = new float[_rings_per_segment];
        var inBetweenRingsPerSegmentHalf = (_rings_per_segment - 3) / 2;
        
        zPositionPerRing[0] = genome.Length / 2;
        zPositionPerRing[_rings_per_segment -1] = - genome.Length / 2;
        zPositionPerRing[inBetweenRingsPerSegmentHalf + 1 ] = bulgeZPosition;
        
        var startToBulgeStep = (genome.Length / 2  -  bulgeZPosition) / (endToBulgeDistance - startToBulgeDistance +1 );
        var endToBulgeStep = (bulgeZPosition -  (genome.Length / 2) ) / (endToBulgeDistance - startToBulgeDistance +1 );
        for (var i = 1; i < inBetweenRingsPerSegmentHalf + 1; i++)
        {
            zPositionPerRing[i] = zPositionPerRing[0] - startToBulgeStep * i;
            zPositionPerRing[_rings_per_segment - 1 - i] = zPositionPerRing[_rings_per_segment - 1] + endToBulgeStep * i
        }

        return zPositionPerRing;
    }
    
    
    public static ArrayMesh CreateMesh(SegmentGenome genome)
    {
        
        float x_to_y_ratio = 1f;
        
        // Arithmetic preparations
        var bulgeRadius = genome.MidBulge * float.Max(genome.StartRadius, genome.EndRadius);
        
        
        var zPositionsPerRinge = CalculateRingZPositions(genome, bulgeRadius);
        
        var xRadiusPerRing = new float[_rings_per_segment];
        var yRadiusPerRing = new float[_rings_per_segment];

        

        
        
        
        // TODO: Implement procedural mesh generation based on gene parameters
        var surface_tool = new SurfaceTool();
        surface_tool.Begin(Mesh.PrimitiveType.Triangles);
    }
}