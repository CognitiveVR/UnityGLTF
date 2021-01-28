using System.Collections.Generic;
using System.Linq;
using GLTF.Extensions;
using GLTF.Math;
using Newtonsoft.Json;

namespace GLTF.Schema
{
	/// <summary>
	/// A node in the node hierarchy.
	/// When the node contains `skin`, all `mesh.primitives` must contain `JOINT`
	/// and `WEIGHT` attributes.  A node can have either a `matrix` or any combination
	/// of `translation`/`rotation`/`scale` (TRS) properties.
	/// TRS properties are converted to matrices and postmultiplied in
	/// the `T * R * S` order to compose the transformation matrix;
	/// first the scale is applied to the vertices, then the rotation, and then
	/// the translation. If none are provided, the transform is the Identity.
	/// When a node is targeted for animation
	/// (referenced by an animation.channel.target), only TRS properties may be present;
	/// `matrix` will not be present.
	/// </summary>
	public class GLTFAreaLight : GLTFLight
	{
		public float height;
		public float width;

		public GLTFAreaLight()
		{
		}

		public GLTFAreaLight(GLTFAreaLight node, GLTFRoot gltfRoot) : base(node, gltfRoot)
		{
			if (node == null) return;
		}

		public new static GLTFAreaLight Deserialize(GLTFRoot root, JsonReader reader)
		{
			var node = new GLTFAreaLight();

			while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
			{
				var curProp = reader.Value.ToString();

				switch (curProp)
				{
					case "type":
						node.type = reader.ReadAsString();
						break;
					case "color":
						node.color = reader.ReadAsRGBAColor();
						break;
					case "intensity":
						node.intensity = (float)reader.ReadAsDouble();
						break;
					case "width":
						node.width = (float)reader.ReadAsDouble();
						break;
					case "height":
						node.height = (float)reader.ReadAsDouble();
						break;
					case "name":
						node.name = reader.ReadAsString();
						break;
				}
			}

			return node;
		}

		public override void Serialize(JsonWriter writer)
		{
			writer.WriteStartObject();

			writer.WritePropertyName("type");
			writer.WriteValue(type);

			if (intensity != 1.0f)
			{
				writer.WritePropertyName("intensity");
				writer.WriteValue(intensity);
			}
			if (!string.IsNullOrEmpty(name))
			{
				writer.WritePropertyName("name");
				writer.WriteValue(name);
			}

			writer.WritePropertyName("width");
			writer.WriteValue(width);

			writer.WritePropertyName("height");
			writer.WriteValue(height);

			writer.WritePropertyName("color");
			writer.WriteStartArray();
			writer.WriteValue(color.R);
			writer.WriteValue(color.G);
			writer.WriteValue(color.B);
			//writer.WriteValue(color.A);
			writer.WriteEndArray();

			//base.Serialize(writer);

			writer.WriteEndObject();
		}
	}
}
