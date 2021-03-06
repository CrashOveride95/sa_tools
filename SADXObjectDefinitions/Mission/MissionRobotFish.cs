﻿using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SonicRetro.SAModel;
using SonicRetro.SAModel.Direct3D;
using SonicRetro.SAModel.SAEditorCommon.DataTypes;
using SonicRetro.SAModel.SAEditorCommon.SETEditing;
using System.Collections.Generic;

namespace SADXObjectDefinitions.Mission
{
	class MissionRobotFish : ObjectDefinition
	{
		private NJS_OBJECT model;
		private Mesh[] meshes;

		public override void Init(ObjectData data, string name, Device dev)
		{
			model = ObjectHelper.LoadModel("Objects/Mission/Mission Robot Fish.sa1mdl");
			meshes = ObjectHelper.GetMeshes(model, dev);
		}

		public override HitResult CheckHit(SETItem item, Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View, MatrixStack transform)
		{
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateY((ushort)(item.Rotation.Y + 0x8000));
			transform.NJRotateZ(item.Rotation.Z);
			transform.NJRotateX(item.Rotation.X);
			transform.NJTranslate(-0.5f, 0, 0);
			HitResult result = model.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes);
			transform.Pop();
			return result;
		}

		public override List<RenderInfo> Render(SETItem item, Device dev, EditorCamera camera, MatrixStack transform)
		{
			List<RenderInfo> result = new List<RenderInfo>();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateY((ushort)(item.Rotation.Y + 0x8000));
			transform.NJRotateZ(item.Rotation.Z);
			transform.NJRotateX(item.Rotation.X);
			transform.NJTranslate(-0.5f, 0, 0);
			result.AddRange(model.DrawModelTree(dev, transform, ObjectHelper.GetTextures("mecha"), meshes));
			if (item.Selected)
				result.AddRange(model.DrawModelTreeInvert(transform, meshes));
			transform.Pop();
			return result;
		}

		public override BoundingSphere GetBounds(SETItem item)
		{
			MatrixStack transform = new MatrixStack();
			transform.NJTranslate(item.Position);
			transform.NJRotateY((ushort)(item.Rotation.Y + 0x8000));
			transform.NJRotateZ(item.Rotation.Z);
			transform.NJRotateX(item.Rotation.X);
			transform.NJTranslate(-0.5f, 0, 0);
			return ObjectHelper.GetModelBounds(model, transform);
		}

		public override string Name { get { return "Mission Robot Fish"; } }

		static object GetWeight(SETItem obj)
		{
			MissionSETItem item = (MissionSETItem)obj;
			return (ushort)(item.PRMBytes[4] << 8 | item.PRMBytes[5]);
		}

		static void SetWeight(SETItem obj, object val)
		{
			MissionSETItem item = (MissionSETItem)obj;
			ushort value = (ushort)val;
			item.PRMBytes[4] = (byte)(value >> 8);
			item.PRMBytes[5] = (byte)value;
		}

		private PropertySpec[] customProperties = new PropertySpec[] {
			new PropertySpec("Weight", typeof(short), null, null, -1, GetWeight, SetWeight)
		};

		public override PropertySpec[] CustomProperties { get { return customProperties; } }
	}
}
