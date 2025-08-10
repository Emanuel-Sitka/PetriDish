
using PetriDish.Generation;
using PetriDish.Genome;

namespace PetriDish.BioMorphs;

using Godot;

[Tool]
[GlobalClass]
public partial class ProceduralSegment : MeshInstance3D
{
    [Export] public float Length { get; set; }
    [Export] public float StartRadius { get; set; }
    [Export] public float EndRadius { get; set; }
    [Export] public float MidBulge { get; set; }
    
    public override void _Ready()
    {
        var genome = new SegmentGenome
        {
            Length = this.Length,
            StartRadius = this.StartRadius,
            EndRadius = this.EndRadius,
            MidBulge = this.MidBulge
        };

        this.Mesh = SegmentMeshFactory.CreateMesh(genome, 11, 16);
    }
}