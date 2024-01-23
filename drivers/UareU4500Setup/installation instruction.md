https://answers.microsoft.com/en-us/windows/forum/windows_11-hardware/win-11-driver-install-issues/569d184e-57bc-4269-9ab7-daa91a76c46f?messageId=4fb1f526-a732-46ab-8ca9-1ec477e68fcd

https://www.winhelponline.com/blog/run-program-as-trustedinstaller-locked-registry-keys-files/?expand_article=1#advancedrun

Steps to intall
- 
1. run `LaunchTrustredInstaller.cmd`
2. In the TrustedInstaller Command Prompt, run:
    - `icacls c:\windows\system32\en-us /grant administrators:F`
    - `icacls c:\windows\system32\en-us /grant "NT AUTHORITY\SYSTEM":F`