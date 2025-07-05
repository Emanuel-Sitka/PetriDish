using System;
using PetriDish.Genome;
using Godot;
namespace PetriDish.Generation;

public static class SegmentMeshFactory
{
    private static int _verticesPerRing = 16;
    private static int _rings_per_segment = 9;
        
    public static ArrayMesh CreateMesh(SegmentGenome genome)
    {
        
        float x_to_y_ratio = 1f;
        
        // Arithmetic preparations
        var bulgeRadius = genome.MidBulge * float.Max(genome.StartRadius, genome.EndRadius);
        
        var startToBulgeDistance = genome.Length * float.Sqrt(bulgeRadius *  bulgeRadius - genome.StartRadius * genome.StartRadius)
            / (float.Sqrt(bulgeRadius *  bulgeRadius - genome.StartRadius * genome.StartRadius) 
               +  float.Sqrt(bulgeRadius *  bulgeRadius - genome.EndRadius * genome.EndRadius)); 
        
        var endToBulgeDistance = genome.Length - startToBulgeDistance;
        
        var bulge_z_position = genome.Length / 2 - startToBulgeDistance;
        
        // Calculating the z_positions of the vertex rings
        var in_between_rings_per_segment_half = (_rings_per_segment - 3) / 2;
        
        
        // TODO: Implement procedural mesh generation based on gene parameters
        var surface_tool = new SurfaceTool();
        surface_tool.Begin(Mesh.PrimitiveType.Triangles);
    }
}