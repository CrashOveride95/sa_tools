﻿using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SonicRetro.SAModel;
using SonicRetro.SAModel.Direct3D;
using SonicRetro.SAModel.SAEditorCommon.DataTypes;
using SonicRetro.SAModel.SAEditorCommon.SETEditing;
using System;

namespace SADXObjectDefinitions.Common
{
	public class Cube : ObjectDefinition
	{
		private NJS_OBJECT model;
		private Mesh[] meshes;

		public override void Init(ObjectData data, string name, Device dev)
		{
			model = ObjectHelper.LoadModel("Objects/Collision/C CUBE.sa1mdl");
			meshes = ObjectHelper.GetMeshes(model, dev);
		}

		public override HitResult CheckHit(SETItem item, Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View, MatrixStack transform)
		{
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateY(item.Rotation.Y);
			transform.NJScale((item.Scale.X + 10) / 5f, (item.Scale.Y + 10) / 5f, (item.Scale.Z + 10) / 5f);
			HitResult result = model.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes);
			transform.Pop();
			return result;
		}

		public override List<RenderInfo> Render(SETItem item, Device dev, EditorCamera camera, MatrixStack transform)
		{
			List<RenderInfo> result = new List<RenderInfo>();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateY(item.Rotation.Y);
			transform.NJScale((item.Scale.X + 10) / 5f, (item.Scale.Y + 10) / 5f, (item.Scale.Z + 10) / 5f);
			result.AddRange(model.DrawModelTree(dev, transform, null, meshes));
			if (item.Selected)
				result.AddRange(model.DrawModelTreeInvert(transform, meshes));
			transform.Pop();
			return result;
		}

		public override BoundingSphere GetBounds(SETItem item)
		{
			MatrixStack transform = new MatrixStack();
			transform.NJTranslate(item.Position);
			transform.NJRotateY(item.Rotation.Y);
			transform.NJScale((item.Scale.X + 10) / 5f, (item.Scale.Y + 10) / 5f, (item.Scale.Z + 10) / 5f);
			return ObjectHelper.GetModelBounds(model, transform, Math.Max(Math.Max((item.Scale.X + 10) / 5f, (item.Scale.Y + 10) / 5f), (item.Scale.Z + 10) / 5f));
		}

		public override string Name { get { return "Solid Cube"; } }
	}
}