import hashlib
import os
import datetime

import os
import urllib.request
import tkinter as tk
from tkinter import filedialog
from tkinter import messagebox
from tkinter import ttk

def main():
    f = Files()
    f.writeMalwareData()
    f.setMalwareDatas()
    f.runScan()





class FileFormat:
    def __init__(self, fName, md5):
        self.fName = fName
        self.md5 = md5

class Files:
    def __init__(self):
        self.allSystemFiles = []
        self.MFinSystem = []
        self.MalwareFiles = []

    def runScan(self, file=None):
        self.setMalwareDatas()
        if file is None:
            currentDir = os.path.join("C:", os.sep, "Users", "mgoek", "Desktop", "BIL495_Bitirme_Projesi", "BIL495_Bitirme_Projesi", "TESTET")
        else:
            currentDir = file
        self.displayDirectoryContents(currentDir)
        self.cleanAll()

    def displayDirectoryContents(self, dir):
        try:
            files = os.listdir(dir)
            for file in files:
                path = os.path.join(dir, file)
                if os.path.isdir(path):
                    self.displayDirectoryContents(path)
                else:
                    self.addFile(path)
        except Exception as e:
            pass

    def writeTxt(self, content):
        FILENAME = "print.txt"
        with open(FILENAME, "a") as file:
            file.write(content + "\n")

    def cleanAll(self):
        if len(self.MalwareFiles) >= 1:
            count = len(self.MalwareFiles)
            dateFormat = "%Y/%m/%d %H:%M:%S"
            date = datetime.datetime.now()
            content = "Last Scan Date: " + date.strftime(dateFormat) + "\nFOUND MALWARE(s) COUNT:" + str(count)
            self.writeTxt(content)
            for malwareFile in self.MalwareFiles:
                self.deleteFile(malwareFile.fName)
                content = " **REMOVED! " + " Path: " + malwareFile.fName
                self.writeTxt(content)
        else:
            dateFormat = "%Y/%m/%d %H:%M:%S"
            date = datetime.datetime.now()
            self.writeTxt(" SYSTEM SAFE! " + " Last Scan Date: " + date.strftime(dateFormat))

    def addFile(self, fPat):
        md5 = self.getMD5(fPat)
        MFcheck = self.isMalwareFile(md5)
        if MFcheck:
            self.MalwareFiles.append(FileFormat(fPat, md5))
        self.allSystemFiles.append(FileFormat(fPat, md5))

    def isMalwareFile(self, curFile):
        return self.isElementOfMF(curFile)

    def isElementOfMF(self, newFMd5):
        for temp in self.MFinSystem:
            if temp == newFMd5:
                return True
        return False

    def getMD5(self, path):
        md5 = hashlib.md5()
        with open(path, "rb") as file:
            for chunk in iter(lambda: file.read(4096), b""):
                md5.update(chunk)
        return md5.hexdigest()

    def getNumberofAllFiles(self):
        return len(self.allSystemFiles)

    def getNumberofMFFiles(self):
        return len(self.MFinSystem)

    def updateMF(self):
        directoryPath = os.path.join("C:", os.sep, "Users", "mgoek", "Desktop", "BIL495_Bitirme_Projesi", "BIL495_Bitirme_Projesi", "TESTET")
        if os.path.exists(directoryPath):
            files = os.listdir(directoryPath)
            if files:
                for file in files:
                    print(file)
            else:
                print("Dizin boş.")
        else:
            print("Dizin bulunamadı veya bir dizin değil.")
        try:
            files = os.listdir(directoryPath)
            for file in files:
                path = os.path.join(directoryPath, file)
                if os.path.isfile(path):
                    md5 = self.getMD5(path)
                    self.MFinSystem.append(md5)
        except Exception as e:
            pass

    def writeMalwareData(self):
        FILENAME = "MALWARES.txt"
        self.updateMF()
        with open(FILENAME, "a") as file:
            for md5 in self.MFinSystem:
                file.write(md5 + "\n")

    def setMalwareDatas(self):
        fileName = "MALWARES.txt"
        try:
            with open(fileName, "r") as file:
                lines = file.readlines()
                for line in lines:
                    self.MFinSystem.append(line.strip())
        except Exception as e:
            pass

    def deleteFile(self, filePath):
        try:
            os.remove(filePath)
            return True
        except Exception as e:
            return False
main()


