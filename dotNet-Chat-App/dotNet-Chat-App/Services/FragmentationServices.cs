using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace dotNet_Chat_App.Services
{
	public class FragmentationServices
	{
		/// <summary>
		/// Data fragmentation from an object
		/// </summary>
		/// <param name="data">Data has object type</param>
		/// <returns>A fragmented byte array data</returns>
		public static byte[] Serialize(object data)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, data);

				byte[] bytes = stream.ToArray();
				stream.Flush();

				return bytes;
			}
		}

		/// <summary>
		/// Combine fragmentation from a data object
		/// </summary>
		/// <param name="data">Data has byte array type</param>
		/// <returns>An object data</returns>
		public static object Deserialize(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				stream.Position = 0;
				object desObj = new BinaryFormatter().Deserialize(stream);
				return desObj;
			}
		}

		/// <summary>
		/// Fragment data to stream
		/// </summary>
		/// <param name="data">Data has object type</param>
		/// <returns>A stream that contain the content of data</returns>
		public static MemoryStream SerializeToStream(object data)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, data);
				return stream;
			}
		}

		/// <summary>
		/// Combine fragmentation from a stream into data object
		/// </summary>
		/// <param name="stream">Data has stream type</param>
		/// <returns>An object data</returns>
		public static object DeserializeFromStream(MemoryStream stream)
		{
			IFormatter formatter = new BinaryFormatter();
			stream.Seek(0, SeekOrigin.Begin);
			object data = formatter.Deserialize(stream);
			return data;
		}
	}
}
