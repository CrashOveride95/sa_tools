﻿using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SonicRetro.SAModel;
using SonicRetro.SAModel.Direct3D;
using SonicRetro.SAModel.SAEditorCommon.DataTypes;
using SonicRetro.SAModel.SAEditorCommon.SETEditing;

namespace SADXObjectDefinitions.EmeraldCoast
{
	public abstract class Seagull : ObjectDefinition
	{
		protected NJS_OBJECT model;
		protected Mesh[] meshes;

		public override HitResult CheckHit(SETItem item, Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View, MatrixStack transform)
		{

			transform.Push();
			transform.NJTranslate(item.Position);
			int RotY = item.Rotation.Y + 34;
			if ((RotY + 24) != 0x4000)
			{
				transform.NJRotateY((RotY + 24) - 0x4000);
			}
			transform.NJScale(1.5f, 1.5f, 1.5f);
			HitResult result = model.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes);
			transform.Pop();
			return result;
		}

		public override List<RenderInfo> Render(SETItem item, Device dev, EditorCamera camera, MatrixStack transform)
		{
			List<RenderInfo> result = new List<RenderInfo>();
			transform.Push();
			transform.NJTranslate(item.Position);
			int RotY = item.Rotation.Y + 34;
			if ((RotY + 24) != 0x4000)
			{
				transform.NJRotateY((RotY + 24) - 0x4000);
			}
			transform.NJScale(1.5f, 1.5f, 1.5f);
			result.AddRange(model.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), meshes));
			if (item.Selected)
				result.AddRange(model.DrawModelTreeInvert(transform, meshes));
			transform.Pop();
			return result;
		}

		public override BoundingSphere GetBounds(SETItem item)
		{
			MatrixStack transform = new MatrixStack();
			transform.NJTranslate(item.Position);
			int RotY = item.Rotation.Y + 34;
			if ((RotY + 24) != 0x4000)
			{
				transform.NJRotateY((RotY + 24) - 0x4000);
			}
			transform.NJScale(1.5f, 1.5f, 1.5f);
			return ObjectHelper.GetModelBounds(model, transform);
		}
	}

	public class Kamome : Seagull
	{
		public override void Init(ObjectData data, string name, Device dev)
		{
			model = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/O KAMOME.sa1mdl");
			meshes = ObjectHelper.GetMeshes(model, dev);
		}

		public override string Name { get { return "Seagull (Animated)"; } }
	}

	public class Kamomel : Seagull
	{
		public override void Init(ObjectData data, string name, Device dev)
		{
			model = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/O KAMOMEL.sa1mdl");
			meshes = ObjectHelper.GetMeshes(model, dev);
		}

		public override string Name { get { return "Seagull"; } }
	}
}