﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using ICSharpCode.Core;
using ICSharpCode.PackageManagement.Design;
using ICSharpCode.PackageManagement.EnvDTE;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Workbench;
using NUnit.Framework;
using PackageManagement.Tests.Helpers;
using Rhino.Mocks;

namespace PackageManagement.Tests.EnvDTE
{
	[TestFixture]
	public class Reference3Tests
	{
		Reference3 reference;
		TestableProject msbuildProject;
		FakePackageManagementProjectService fakeProjectService;
		TestableDTEProject project;
		ReferenceProjectItem referenceProjectItem;
		
		void CreateReference(string name)
		{
			project = new TestableDTEProject();
			msbuildProject = project.TestableProject;
			referenceProjectItem = msbuildProject.AddReference(name);
			fakeProjectService = project.FakeProjectService;
			CreateReference(project, referenceProjectItem);
			IWorkbench workbench = MockRepository.GenerateStub<IWorkbench>();
			ICSharpCode.SharpDevelop.SD.Services.AddService(typeof(IWorkbench), workbench);
		}
		
		void CreateReference(Project project, ReferenceProjectItem referenceProjectItem)
		{
			reference = new Reference3(project, referenceProjectItem);
		}
		
		TestableProject CreateProjectReference(string parentProjectFileName, string referencedProjectFileName)
		{
			project = new TestableDTEProject();
			msbuildProject = project.TestableProject;
			msbuildProject.FileName = new FileName(parentProjectFileName);
			TestableProject referencedProject = ProjectHelper.CreateTestProject();
			referencedProject.FileName = new FileName(referencedProjectFileName);
			((ICollection<IProject>)msbuildProject.ParentSolution.Projects).Add(referencedProject);
			IWorkbench workbench = MockRepository.GenerateStub<IWorkbench>();
			ICSharpCode.SharpDevelop.SD.Services.AddService(typeof(IWorkbench), workbench);
			ProjectReferenceProjectItem referenceProjectItem = msbuildProject.AddProjectReference(referencedProject);
			fakeProjectService = project.FakeProjectService;
			CreateReference(project, referenceProjectItem);
			return referencedProject;
		}
		
		[Test]
		public void Name_ReferenceNameIsSystemXml_ReturnsSystemXml()
		{
			CreateReference("System.Xml");
			string name = reference.Name;
			
			Assert.AreEqual("System.Xml", name);
		}
		
		[Test]
		public void Remove_RemoveSystemXmlReferenceFromProject_ProjectReferenceRemoved()
		{
			CreateReference("System.Xml");
			
			reference.Remove();
			
			int count = msbuildProject.Items.Count;
			
			Assert.AreEqual(0, count);
		}
		
		[Test]
		public void Remove_RemoveSystemXmlReferenceFromProject_ProjectIsSaved()
		{
			CreateReference("System.Xml");
			
			reference.Remove();
			
			bool saved = msbuildProject.IsSaved;
			
			Assert.IsTrue(saved);
		}
		
		[Test]
		public void SourceProject_SystemXmlReference_ReturnsNull()
		{
			CreateReference("System.Xml");
			
			global::EnvDTE.Project project = reference.SourceProject;
			
			Assert.IsNull(project);
		}
		
		[Test]
		public void SourceProject_ReferenceIsProjectReference_ReturnsReferencedProject()
		{
			string parentProjectFileName = @"d:\projects\project.csproj";
			string referencedProjectFileName = @"d:\projects\referencedproject.csproj";
			TestableProject referencedProject = CreateProjectReference(parentProjectFileName, referencedProjectFileName);
			
			global::EnvDTE.Project project = reference.SourceProject;
			
			Assert.AreEqual(@"d:\projects\referencedproject.csproj", project.FileName);
		}
		
		[Test]
		public void AutoReferenced_SystemXmlReferenceInProjectReferences_ReturnsFalse()
		{
			CreateReference("System.Xml");
			
			bool result = reference.AutoReferenced;
			
			Assert.IsFalse(result);
		}
		
		[Test]
		public void Path_SystemXmlReferenceInProjectReferences_ReturnsFullPathToSystemXml()
		{
			CreateReference("System.Xml");
			referenceProjectItem.FileName = @"c:\Program Files\Microsoft\Reference Assemblies\v4\System.Xml.dll";
			
			string path = reference.Path;
			
			Assert.AreEqual(@"c:\Program Files\Microsoft\Reference Assemblies\v4\System.Xml.dll", path);
		}
	}
}
