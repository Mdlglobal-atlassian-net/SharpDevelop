﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ICSharpCode.Core;
using ICSharpCode.PythonBinding;
using ICSharpCode.Scripting.Tests.Utils;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Gui;

namespace PythonBinding.Tests.Utils
{
	public class MockWorkbench : IPythonWorkbench
	{		
		public MockPythonConsolePad MockPythonConsolePad = new MockPythonConsolePad();
		public MockEditableViewContent ActiveMockEditableViewContent;
		
		public static MockWorkbench CreateWorkbenchWithOneViewContent(string fileName)
		{
			MockEditableViewContent viewContent = new MockEditableViewContent();
			viewContent.PrimaryFileName = new FileName(fileName);
						
			MockWorkbench workbench = new MockWorkbench();
			workbench.ActiveMockEditableViewContent = viewContent;
			
			return workbench;
		}
		
		public IViewContent ActiveViewContent {
			get { return ActiveMockEditableViewContent; }
		}		
				
		public IPythonConsolePad GetPythonConsolePad()
		{
			return MockPythonConsolePad;
		}
	}
}
