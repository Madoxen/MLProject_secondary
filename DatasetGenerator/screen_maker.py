import os

moviename = ""
def mkdir(name):
    os.system("mkdir "+name)
    global moviename
    moviename = name

def screenshot(name):
    os.system("scrot -z "+os.path.dirname(os.path.abspath(__file__))+"/"+moviename+"/"+name+".png") # z argument prevents beeping