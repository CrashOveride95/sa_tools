﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SA_Tools;
using SonicRetro.SAModel.Direct3D;
using SonicRetro.SAModel.SAEditorCommon.UI;

namespace SonicRetro.SAModel.SAEditorCommon.DataTypes
{
	public class DeathZoneItem : Item
	{
		[Browsable(false)]
		private NJS_OBJECT Model { get; set; }
		[NonSerialized]
		private Mesh mesh;
		[Browsable(false)]
		private Mesh Mesh { get { return mesh; } set { mesh = value; } }

		[Browsable(false)]
		public override BoundingSphere Bounds
		{
			get
			{
				Matrix m = Matrix.Identity;
				Model.ProcessTransforms(m);
				float scale = Math.Max(Math.Max(Model.Scale.X, Model.Scale.Y), Model.Scale.Z);
				return new BoundingSphere(Vector3.TransformCoordinate(Model.Attach.Bounds.Center.ToVector3(), m).ToVertex(), Model.Attach.Bounds.Radius * scale);
			}
		}

		[NonSerialized]
		private Device device;

		public DeathZoneItem(Device device, EditorItemSelection selectionManager)
			: base (selectionManager)
		{
			this.device = device;
			Model = new NJS_OBJECT();
			ImportModel();
			Paste();
		}

		public DeathZoneItem(NJS_OBJECT model, SA1CharacterFlags flags, Device device, EditorItemSelection selectionManager)
			: base(selectionManager)
		{
			Model = model;
			model.ProcessVertexData();
			Flags = flags;
			Mesh = Model.Attach.CreateD3DMesh(device);
			this.device = device;
		}

		public override Vertex Position { get { return Model.Position; } set { Model.Position = value; } }

		public override Rotation Rotation { get { return Model.Rotation; } set { Model.Rotation = value; } }

		public override HitResult CheckHit(Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View)
		{
			return Model.CheckHit(Near, Far, Viewport, Projection, View, Mesh);
		}

		public override List<RenderInfo> Render(Device dev, EditorCamera camera, MatrixStack transform)
		{
			List<RenderInfo> result = new List<RenderInfo>();
			result.AddRange(Model.DrawModel(dev, transform, LevelData.Textures[LevelData.leveltexs], Mesh, false));
			if (Selected)
				result.AddRange(Model.DrawModelInvert(transform, Mesh, false));
			return result;
		}

		public override void Paste()
		{
			if (LevelData.DeathZones != null)
				LevelData.DeathZones.Add(this);
		}

		public override void Delete()
		{
			LevelData.DeathZones.Remove(this);
		}

		[Browsable(true)]
		[DisplayName("Import Model")]
		public void ImportModel()
		{
			OpenFileDialog dlg = new OpenFileDialog() { DefaultExt = "obj", Filter = "OBJ Files|*.obj;*.objf", RestoreDirectory = true };
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				Model.Attach = Direct3D.Extensions.obj2nj(dlg.FileName, LevelData.TextureBitmaps[LevelData.leveltexs].Select(a => a.Name).ToArray());
				Mesh = Model.Attach.CreateD3DMesh(device);
			}
		}

		//[Browsable(true)]
		[DisplayName("Export Model")]
		public void ExportModel()
		{

		}

		public SA1CharacterFlags Flags { get; set; }

		[Browsable(false)]
		public bool Visible
		{
			get
			{
				switch (LevelData.Character)
				{
					case 0:
						return (Flags & SA1CharacterFlags.Sonic) == SA1CharacterFlags.Sonic;
					case 1:
						return (Flags & SA1CharacterFlags.Tails) == SA1CharacterFlags.Tails;
					case 2:
						return (Flags & SA1CharacterFlags.Knuckles) == SA1CharacterFlags.Knuckles;
					case 3:
						return (Flags & SA1CharacterFlags.Amy) == SA1CharacterFlags.Amy;
					case 4:
						return (Flags & SA1CharacterFlags.Gamma) == SA1CharacterFlags.Gamma;
					case 5:
						return (Flags & SA1CharacterFlags.Big) == SA1CharacterFlags.Big;
				}
				return false;
			}
		}

		public bool Sonic
		{
			get
			{
				return (Flags & SA1CharacterFlags.Sonic) == SA1CharacterFlags.Sonic;
			}
			set
			{
				Flags = (Flags & ~SA1CharacterFlags.Sonic) | (value ? SA1CharacterFlags.Sonic : 0);
			}
		}

		public bool Tails
		{
			get
			{
				return (Flags & SA1CharacterFlags.Tails) == SA1CharacterFlags.Tails;
			}
			set
			{
				Flags = (Flags & ~SA1CharacterFlags.Tails) | (value ? SA1CharacterFlags.Tails : 0);
			}
		}

		public bool Knuckles
		{
			get
			{
				return (Flags & SA1CharacterFlags.Knuckles) == SA1CharacterFlags.Knuckles;
			}
			set
			{
				Flags = (Flags & ~SA1CharacterFlags.Knuckles) | (value ? SA1CharacterFlags.Knuckles : 0);
			}
		}

		public bool Amy
		{
			get
			{
				return (Flags & SA1CharacterFlags.Amy) == SA1CharacterFlags.Amy;
			}
			set
			{
				Flags = (Flags & ~SA1CharacterFlags.Amy) | (value ? SA1CharacterFlags.Amy : 0);
			}
		}

		public bool Gamma
		{
			get
			{
				return (Flags & SA1CharacterFlags.Gamma) == SA1CharacterFlags.Gamma;
			}
			set
			{
				Flags = (Flags & ~SA1CharacterFlags.Gamma) | (value ? SA1CharacterFlags.Gamma : 0);
			}
		}

		public bool Big
		{
			get
			{
				return (Flags & SA1CharacterFlags.Big) == SA1CharacterFlags.Big;
			}
			set
			{
				Flags = (Flags & ~SA1CharacterFlags.Big) | (value ? SA1CharacterFlags.Big : 0);
			}
		}

		public DeathZoneFlags Save(string path, int i)
		{
			ModelFile.CreateFile(Path.Combine(path, i.ToString(NumberFormatInfo.InvariantInfo) + ".sa1mdl"), Model, null, null, null, LevelData.LevelName + " Death Zone " + i.ToString(NumberFormatInfo.InvariantInfo), "SADXLVL2", null, ModelFormat.Basic);
			return new DeathZoneFlags() { Flags = Flags };
		}
	}
}
