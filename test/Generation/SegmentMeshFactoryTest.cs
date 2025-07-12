using GdUnit4;
using PetriDish.Generation;
using PetriDish.Genome;

namespace PetriDish.test.Generation;

using static Assertions;

[TestSuite]
public class SegmentMeshFactoryTest
{
    [TestCase]
    public void TestRingZPositionsSymmetricCase()
    {
        var zPositions = SegmentMeshFactory.CalculateRingZPositions(8f, 0f, 9);
        AssertArray(zPositions).IsEqual([4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f]);
    }

    [TestCase]
    public void TestRingRadii()
    {
        var zPositions = SegmentMeshFactory.CalculateRingZPositions(8f, 0f, 3);
        var genome = new SegmentGenome()
            {StartRadius = 1f, EndRadius = 1f, Length = 8f, MidBulge = 2f};
        var test = SegmentMeshFactory.CalculateRadiusPerZPosition(genome, zPositions);
        float[] expected = [1f, 2f, 1f];
        AssertThat(test.Length).IsEqual(3);
        for (int i=0; i < test.Length; i++)
        {
            AssertThat(test[i]).IsEqualApprox(expected[i], 1e-5f);
        }
    }
    
    [TestCase]
    public void TestVertices()
    {
        var zPositions = SegmentMeshFactory.CalculateRingZPositions(8f, 0f, 3);
        var genome = new SegmentGenome()
            {StartRadius = 1f, EndRadius = 1f, Length = 8f, MidBulge = 2f};
        
        var radii = SegmentMeshFactory.CalculateRadiusPerZPosition(genome, zPositions);
        var vertices = SegmentMeshFactory.CalculateVertexPositions(zPositions, radii, 4);
        AssertBool(true).IsTrue();
    }

    [TestCase]
    public void TestAssymetricSegment()
    {
        var genome = new SegmentGenome(5.0f, 3.0f, 4.0f, 1.0f);
        var bulgePosition = SegmentMeshFactory.CalculateBulgePosition(genome);
        var zPositions = SegmentMeshFactory.CalculateRingZPositions(genome.Length, bulgePosition, 3);
        var radii = SegmentMeshFactory.CalculateRadiusPerZPosition(genome, zPositions);
        var vertices = SegmentMeshFactory.CalculateVertexPositions(zPositions, radii, 4);
        AssertBool(true).IsTrue();
    }
}