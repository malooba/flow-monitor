!include MUI2.nsh

Name "FlowMonitor"
OutFile "FlowMonitor.exe"

!include "Sections.nsh"
!include "logiclib.nsh"

InstallDir $PROGRAMFILES\Malooba\FlowMonitor
#RequestExecutionLevel admin

ShowInstDetails show
ShowUnInstDetails show

!define MUI_ICON "flow.ico"
!define MUI_UNICON "flow.ico"

!insertmacro MUI_PAGE_DIRECTORY

#!insertmacro MUI_PAGE_COMPONENTS

!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"

# Debug or Release Build
!define BUILD "Debug"

Section
  SectionIn RO
  SetOutPath "$instdir"
  WriteUninstaller "uninstall.exe"

SectionEnd

Section "FlowMonitor" FlowMonitor
  SectionIn RO
  SetOutPath "$INSTDIR"
  File "FlowMonitor\bin\${BUILD}\FlowMonitor.exe"
  File "FlowMonitor\bin\${BUILD}\FlowMonitor.exe.config"
  File "FlowMonitor\bin\${BUILD}\Diagram.dll"
  File "FlowMonitor\bin\${BUILD}\Newtonsoft.Json.dll"
  File "FlowMonitor\bin\${BUILD}\ScintillaNet.dll"
  File "FlowMonitor\bin\${BUILD}\palette.json"

  StrCmp ${BUILD} "Release" release 0
  File "FlowMonitor\bin\${BUILD}\FlowMonitor.pdb"
  File "FlowMonitor\bin\${BUILD}\Diagram.pdb"
  release:
SectionEnd

Section "Uninstall"
  SectionIn RO
  RMDir /r /REBOOTOK "$INSTDIR"
  Delete "$INSTDIR\uninstall.exe"
SectionEnd