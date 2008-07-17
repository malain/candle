﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// This file contains generated default command definitions.
// Additional command definitions should be added to CustomCmd.ctc.
//

///////////////////////////////////////////////////////////////////////////////
// CTC command IDs - MUST be kept in sync with Constants.cs

#define menuidContext		0x10000
#define grpidContextMain	0x20000
#define menuidExplorer		0x10001
#define cmdidViewExplorer	0x0001

///////////////////////////////////////////////////////////////////////////////
// CTC macros

#define OI_NOID		guidOfficeIcon:msotcidNoIcon
#define DIS_DEF		DEFAULTDISABLED | DEFAULTINVISIBLE | DYNAMICVISIBILITY
#define VIS_DEF		COMMANDWELLONLY	

///////////////////////////////////////////////////////////////////////////////
// Menu definitions

#define GENERATED_MENUS \
	guidCmdSet:menuidContext, guidCmdSet:menuidContext,	0x0000,	CONTEXT|ALWAYSCREATE, "Candle Designer Context Menu", "Candle Context"; \
	guidCmdSet:menuidExplorer, guidCmdSet:menuidExplorer, 0x0000, CONTEXT|ALWAYSCREATE, "Candle Explorer Context Menu", "Component Explorer"; \

///////////////////////////////////////////////////////////////////////////////
// Group definitions

#define GENERATED_GROUPS \
	guidCmdSet:grpidContextMain, guidCmdSet:grpidContextMain,	0x0010; \


///////////////////////////////////////////////////////////////////////////////
// Command definitions

#define GENERATED_BUTTONS \
	guidCmdSet:cmdidViewExplorer, guidSHLMainMenu:IDG_VS_WNDO_OTRWNDWS1, 0x0100,	OI_NOID, BUTTON, DIS_DEF, "Component Explorer";


///////////////////////////////////////////////////////////////////////////////
// Command placement definitions

#define GENERATED_CMDPLACEMENT \
	guidVSStd97:cmdidDelete, guidCmdSet:grpidContextMain, 0x0010; \
	guidSHLMainMenu:IDG_VS_CTXT_SOLUTION_PROPERTIES, guidCmdSet:menuidContext, 0x0500; \
	guidCmdSet:grpidContextMain, guidCmdSet:menuidContext, 0x0010; \
	guidCommonModelingMenu:grpidCompartmentShapeMenuGroup, guidCmdSet:menuidContext, 0x0008; \
	guidCommonModelingMenu:grpidSwimlaneShapeMenuGroup, guidCmdSet:menuidContext, 0x0008; \
	guidCommonModelingMenu:grpidValidateCommands, guidCmdSet:menuidContext, 0x0020; \
	guidCommonModelingMenu:grpidLayoutMenuGroup, guidCmdSet:menuidContext, 0x0030; \
	guidCommonModelingMenu:grpidExplorerMenuGroup, guidCmdSet:menuidExplorer, 0x0010; \
	guidSHLMainMenu:IDG_VS_CTXT_SOLUTION_PROPERTIES, guidCmdSet:menuidExplorer, 0x0020; \
	guidCommonModelingMenu:grpidValidateCommands, guidCmdSet:menuidExplorer, 0x0030; \
    

///////////////////////////////////////////////////////////////////////////////
// Command visibility definitions

#define GENERATED_VISIBILITY \
	guidCmdSet:cmdidViewExplorer, guidEditor; \

///////////////////////////////////////////////////////////////////////////////
// CTC guids - MUST be kept in sync with GeneratedCmd.cs

#define guidPkg			{ 0xacd37819, 0xaa0b, 0x4ba5, { 0xa4, 0x7b, 0xee, 0x76, 0x8b, 0x93, 0xd9, 0x3e } }
#define guidEditor		{ 0xa347c751, 0x7722, 0x4fa1, { 0xb7, 0x3e, 0x2e, 0x03, 0xdb, 0x41, 0xd1, 0xc9 } }



#define guidCmdSet		{ 0x47747347, 0x8334, 0x4049, { 0xac, 0x3f, 0x61, 0x64, 0xb9, 0xc7, 0xc8, 0xdd } }
#define guidEditor		{ 0xa347c751, 0x7722, 0x4fa1, { 0xb7, 0x3e, 0x2e, 0x03, 0xdb, 0x41, 0xd1, 0xc9 } }

// Ajout d'un bouton dans le solution explorer
#define SolutionExplorerToolBar			0x9000
#define SolutionExplorerToolBarGroup	0x9001
#define modelsGuidEditor { 0x56AF6F2B, 0xEF94, 0x4297, { 0x98, 0x57, 0x86, 0x53, 0xA0, 0xAE, 0x02, 0xD8 } }

// Ajout du menu de candle
#define CandleTopLevelMenu              0xA000
#define CandleTopLevelMenuGroup         0xA001
#define VSTopLevelGroup                 0xA002

#define DiagramToolBar                  0xB000
#define DiagramToolBarGroup             0xB001