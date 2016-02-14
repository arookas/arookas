workspace "arookas"
	configurations { "Debug", "Release" }
	targetdir "bin/%{cfg.buildcfg}"
	
	filter "configurations:Debug"
		defines { "DEBUG" }
		flags { "Symbols" }
	
	filter "configurations:Release"
		defines { "RELEASE" }
		optimize "On"
	
	project "arookas"
		kind "SharedLib"
		language "C#"
		namespace "arookas"
		location "arookas"
		
		links { "System", "System.Drawing", "System.Xml", "System.Xml.Linq" }
		
		files {
			"arookas/**.cs",
		}
		
		excludes {
			"arookas/bin/**",
			"arookas/obj/**",
		}
