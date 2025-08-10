using System;
using PetriDish.Genome;
using Godot;
namespace PetriDish.Generation;

public static class SegmentMeshFactory
{
    private const int DefaultRadialResolution = 16;
    private const int DefaultZResolution = 9;


    public static float[] CalculateRingZPositions(float length, float bulgePosition, int zResolution)
    {
        
        var zPositionPerRing = new float[zResolution];
        var midIndex = (zResolution - 1) / 2;
        
        zPositionPerRing[0] = length / 2;
        zPositionPerRing[zResolution -1] = - length / 2;
        zPositionPerRing[midIndex] = bulgePosition;
        
        var startToBulgeStep = (length / 2  -  bulgePosition) / (midIndex);
        var endToBulgeStep = (bulgePosition +  (length / 2)) / (midIndex);
        for (var i = 1; i < midIndex; i++)
        {
            zPositionPerRing[i] = zPositionPerRing[0] - startToBulgeStep * i;
            zPositionPerRing[zResolution - 1 - i] =
                zPositionPerRing[zResolution - 1] + endToBulgeStep * i;
        }

        return zPositionPerRing;
    }

    
    public static float[] CalculateRadiusPerZPosition(SegmentGenome genome, float[] zPositions)
    {
        var bulgePosition = zPositions[(zPositions.Length - 1) / 2];
        var radii = new float[zPositions.Length];
        var bulgeRadius = genome.MidBulge * float.Max(genome.StartRadius, genome.EndRadius);
        var ellipsisLength = bulgeRadius/MathF.Sqrt(bulgeRadius * bulgeRadius - genome.StartRadius * genome.StartRadius) *
                             (genome.Length / 2 - bulgePosition);

        for (var i = 0; i < zPositions.Length; i++)
        {
            var distToBulge = zPositions[i] - bulgePosition;
            radii[i] = bulgeRadius * MathF.Sqrt(ellipsisLength * ellipsisLength - distToBulge * distToBulge) / ellipsisLength;
        }

        return radii;
    }
     

    public static Vector3[] CalculateVertexPositions(float[] zPositionsPerRing, float[] radiusPerZPosition, int radialResolution)
    {
        float xToYRatio = 1f;
        var angularIncrement =  2 * MathF.PI / radialResolution;
        
        
        var vertexPositions = new Vector3[zPositionsPerRing.Length*radialResolution + 2];

        vertexPositions[0] = new Vector3(0f, 0f, zPositionsPerRing[0]);
        vertexPositions[zPositionsPerRing.Length*radialResolution + 1] = new Vector3(0f, 0f, zPositionsPerRing[^1]);
        
        for (int i = 0; i < zPositionsPerRing.Length; i++)
        {
            var zPosition = zPositionsPerRing[i];
            var radius = radiusPerZPosition[i];
            for (int j = 0; j < radialResolution; j++)
            {
                vertexPositions[i * radialResolution +  j + 1] = new Vector3(xToYRatio * radius *  MathF.Cos(angularIncrement * j), 
                    radius *  MathF.Sin(angularIncrement * j), 
                    zPosition);
            }
        }

        return vertexPositions;
    }
 
    public static float CalculateBulgePosition(SegmentGenome genome)
    {
        var bulgeRadius = genome.MidBulge * float.Max(genome.StartRadius, genome.EndRadius);
        var startToBulgeDistance =  genome.Length * float.Sqrt(bulgeRadius *  bulgeRadius - genome.StartRadius * genome.StartRadius)
                                   / (float.Sqrt(bulgeRadius *  bulgeRadius - genome.StartRadius * genome.StartRadius) 
                                      +  float.Sqrt(bulgeRadius *  bulgeRadius - genome.EndRadius * genome.EndRadius)); 
        var bulgePosition = genome.Length / 2 - startToBulgeDistance;
        return bulgePosition;
    }
    
    public static ArrayMesh CreateMesh(SegmentGenome genome, int zResolution = DefaultZResolution, int radialResolution = DefaultRadialResolution)
    {
        var bulgePosition = CalculateBulgePosition(genome);
        var zPositionsPerRing = CalculateRingZPositions(genome.Length, bulgePosition, zResolution);
        var radiiPerZPosition = CalculateRadiusPerZPosition(genome, zPositionsPerRing);
        var vertexPositions = CalculateVertexPositions(zPositionsPerRing, radiiPerZPosition, radialResolution);
        
        var surfaceTool = new SurfaceTool();
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        foreach (var vertexPosition in vertexPositions)
        {
            surfaceTool.AddVertex(vertexPosition);
        }
        
        // Connecting start cap to first vertex ring
        var startCap = 0;
        for (int i = 0; i < radialResolution; i++)
        {
            int current = i + 1;
            int next = (i+1) %  radialResolution + 1;
            
            surfaceTool.AddIndex(startCap);
            surfaceTool.AddIndex(next);
            surfaceTool.AddIndex(current);
        }

        // Connecting vertex rings
        for (int ringIndex = 0; ringIndex < zResolution - 1; ringIndex++)
        {
            for (int i = 0; i < radialResolution; i++)
            {
                int current = ringIndex * radialResolution + i + 1;
                int next = ringIndex * radialResolution + (i+1) %  radialResolution + 1;
                int currentOnNextRing = (ringIndex + 1) * radialResolution + i + 1;
                int nextOnNextRing = (ringIndex+1) * radialResolution + (i+1) %  radialResolution + 1;

                surfaceTool.AddIndex(current);
                surfaceTool.AddIndex(next);
                surfaceTool.AddIndex(nextOnNextRing);
                
                surfaceTool.AddIndex(current);
                surfaceTool.AddIndex(nextOnNextRing);
                surfaceTool.AddIndex(currentOnNextRing);
                
            }
        }
        
        // Connecting last vertex ring to end cap

        var endCap = vertexPositions.Length - 1;
        for (int i = 0; i < radialResolution; i++)
        {
            int current = (zResolution - 1) * radialResolution + i + 1;
            int next = (zResolution - 1) * radialResolution + (i+1) %  radialResolution + 1;
            
            surfaceTool.AddIndex(current);
            surfaceTool.AddIndex(next);
            surfaceTool.AddIndex(endCap);
        }
        
        surfaceTool.GenerateNormals();
        return surfaceTool.Commit();
    }
}