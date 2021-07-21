using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace coreService
{
	/// <summary>
	/// A few helper methods for XML conversions.
	/// </summary>
	public class XmlUtils
	{
		private XmlUtils()
		{
		}

		public static string ToShortDateString( DateTime? dt )
		{
			if( dt.HasValue )
			{
				return dt.Value.ToShortDateString();
			}
			else
			{
				return "";
			}
		}
		
		public static string ToString( object o )
		{
			if( o == null )
			{
				return null;
			}
			else if( o is XmlNode[] )
			{
				StringBuilder builder = new StringBuilder();
				foreach( XmlNode node in (XmlNode[]) o )
				{
					builder.Append( node.OuterXml );
				}
				
				return builder.ToString();
			}
			else if( o.GetType().Name.Equals( "Object" ) )
			{
				return "";
			}
			else
			{
				return o.ToString();
			}
		}

		public static XmlNode FromString( string text )
		{
			XmlTextReader xmlReader = new XmlTextReader( new StringReader( "<X>" + text + "</X>" ) );
			XmlDocument xmlDocument = new XmlDocument();
			XmlNode node = xmlDocument.ReadNode( xmlReader );

			return node;
		}
	}
}
