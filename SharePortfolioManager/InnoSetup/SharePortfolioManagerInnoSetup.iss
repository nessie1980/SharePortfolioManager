; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "SharePortfolioManager"
#define MyAppVersion "0.5.8.1"
#define MyAppPublisher "Thomas Barth"
#define MyAppURL "https://github.com/nessie1980/SharePortfolioManager"
#define MyAppExeName "SharePortfolioManager.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{F6540755-C324-4031-A4FF-192375B7A199}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

LicenseFile=InnoSetup\license.txt
DisableWelcomePage=no
UsePreviousAppDir=no
DefaultDirName={commonpf}\{#MyAppName}
DisableProgramGroupPage=yes
DisableDirPage=no

WizardStyle=modern

SourceDir=..\
PrivilegesRequired=poweruser

OutputDir=Installer
OutputBaseFilename=SharePortfolioManagerInstaller_{#MyAppVersion}

Compression=lzma
SolidCompression=yes
MinVersion=6.1.7600

UninstallDisplayName=Uninstall {#MyAppName}
UninstallFilesDir={app}\uninst

[Types]
Name: "full"; Description: "Full installation"
Name: "compact"; Description: "Compact installation"
Name: "custom"; Description: "Custom installation"; Flags: iscustom

[Components]
Name: "application"; Description: "Application files"; Types: full compact custom; Flags: fixed
Name: "sounds"; Description: "Sound files"; Types: full compact custom; Flags: fixed
Name: "tools"; Description: "Additionals tools"; Types: full;

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "bin\Release\SharePortfolioManager.exe"; DestDir: "{app}"; Components: application; Flags: ignoreversion
Source: "Config\*"; DestDir: "{app}\Settings"; Components: application; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "DLLs\LanguageHandler.dll"; DestDir: "{app}"; Components: application; Flags: ignoreversion
Source: "DLLs\Logger.dll"; DestDir: "{app}"; Components: application; Flags: ignoreversion
Source: "DLLs\Parser.dll"; DestDir: "{app}"; Components: application; Flags: ignoreversion

Source: "Sounds\Error.wav"; DestDir: "{app}\Sound"; Components: sounds; Flags: ignoreversion
Source: "Sounds\UpdateFinished.wav"; DestDir: "{app}\Sound"; Components: sounds; Flags: ignoreversion

Source: "Tools\xpdf-tools-win-4.00\bin32\pdftotext.exe"; Components: tools; DestDir: "{app}\Tools\"; Flags: ignoreversion
Source: "Tools\xpdf-tools-win-4.00\ANNOUNCE"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "Tools\xpdf-tools-win-4.00\CHANGES"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "Tools\xpdf-tools-win-4.00\COPYING"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "Tools\xpdf-tools-win-4.00\COPYING3"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "Tools\xpdf-tools-win-4.00\INSTALL"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "Tools\xpdf-tools-win-4.00\README"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Dirs]
Name: "{userdocs}\{#MyAppName}\Portfolios"; Flags: uninsalwaysuninstall

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}\AppName"
Type: filesandordirs; Name: "{app}\Logs"
Type: filesandordirs; Name: "{app}\Settings"
Type: filesandordirs; Name: "{app}\Sounds"
Type: filesandordirs; Name: "{app}\Tools"
Type: filesandordirs; Name: "{userdocs}\{#MyAppName}\Portfolios"