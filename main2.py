import pefile
name=input("taranacak dosya adi   : ")
pe = pefile.PE(name)
for entry in pe.DIRECTORY_ENTRY_IMPORT:
 print (entry.dll)
 for function in entry.imports:
	 print ('\t',function.name)
