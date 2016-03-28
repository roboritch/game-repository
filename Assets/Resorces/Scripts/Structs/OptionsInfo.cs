using UnityEngine;
using System;
using System.Xml.Serialization;

[XmlRoot("options")]
[Serializable]
public struct OptionsInfo{
	[XmlAttribute("resolution")]
	public Resolution resolution;
}