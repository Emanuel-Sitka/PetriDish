
using PetriDish.Generation;
using PetriDish.Genome;

namespace PetriDish.BioMorphs;

using Godot;

[Tool]
[GlobalClass]
public partial class Segment : MeshInstance3D
{
    public float Length { get; set; }
    public float StartRadius { get; set; }
    public float EndRadius { get; set; }
    public float MidBulge { get; set; }

    public void Configure(SegmentGenome genome)
    {            
        Length = genome.Length;
        StartRadius = genome.StartRadius;
        EndRadius = genome.EndRadius;
        MidBulge = genome.MidBulge;
        
        this.Mesh = SegmentMeshFactory.CreateMesh(genome, 11, 16);
    }
    
    public override void _Ready()
    {
    }
}