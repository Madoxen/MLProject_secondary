import netflix_controls as nc
import screen_maker as sm
import sys
import time

begin_offset = 0 #seconds

name = ""
try:
    name = sys.argv[1]
except:
    print("no movie name given!")

if name is not "":
    print("starting taking screenshots for movie: "+name)
    print("leave stopped movie on fulscreen at 0:0")
    for i in range(10,-1,-1):
        time.sleep(1)
        print("starting in "+str(i)+" !")

    print("here we go!!!")
    
    sm.mkdir(name)

    nc.play_pause()
    nc.play_pause()
    nc.forward(int(begin_offset/10))

    i = 0
    while True:
        nc.wait_for_hide()
        sm.screenshot(name+"_"+str(i))
        nc.forward()
        nc.forward()
        i+=1
        

