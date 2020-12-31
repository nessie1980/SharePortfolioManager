; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "SharePortfolioManager"
#define MyAppVersion "0.5.7.0"
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
DefaultDirName={userpf}\{#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
OutputDir=E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\Installer
OutputBaseFilename=SharePortfolioManagerInstaller_{#MyAppVersion}
Compression=lzma
SolidCompression=yes
MinVersion=6.1.7600
UninstallDisplayName=Uninstall SharePortfolioManager
UninstallFilesDir={app}\uninst

[Types]
Name: "full"; Description: "Full installation"
Name: "compact"; Description: "Compact installation"
Name: "custom"; Description: "Custom installation"; Flags: iscustom

[Components]
Name: "main"; Description: "Main files"; Types: full compact custom; Flags: fixed
Name: "tools"; Description: "Additionals tools"; Types: full;

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\bin\Release\SharePortfolioManager.exe"; DestDir: "{app}"; Components: main; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\Config\*"; DestDir: "{app}\Settings"; Components: main; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\DLLs\LanguageHandler.dll"; DestDir: "{app}"; Components: main; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\DLLs\Logger.dll"; DestDir: "{app}"; Components: main; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\DLLs\Parser.dll"; DestDir: "{app}"; Components: main; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\Tools\xpdf-tools-win-4.00\bin32\pdftotext.exe"; Components: tools; DestDir: "{app}\Tools\"; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\Tools\xpdf-tools-win-4.00\ANNOUNCE"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\Tools\xpdf-tools-win-4.00\CHANGES"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\Tools\xpdf-tools-win-4.00\COPYING"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\Tools\xpdf-tools-win-4.00\COPYING3"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\Tools\xpdf-tools-win-4.00\INSTALL"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
Source: "E:\Programmierung\GitHub\Repos\SharePortfolioManager\SharePortfolioManager\Tools\xpdf-tools-win-4.00\README"; DestDir: "{app}\Tools"; Components: tools; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}\AppName"
Type: filesandordirs; Name: "{app}\Settings"
Type: filesandordirs; Name: "{app}\Tools"
