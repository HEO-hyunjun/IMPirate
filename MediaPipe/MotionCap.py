import cv2
import socket
from cvzone.PoseModule import PoseDetector

width, height = 1280, 720

cap = cv2.VideoCapture(0)
cap.set(3,1280)
cap.set(4,720)

detector = PoseDetector()
posList = []


#통신 부분 시작
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM) #UDP 통신할거임
serverAddressPort = ("127.0.0.1", 5052)

while True:
    success, img = cap.read()
    img = detector.findPose(img)
    Landmark_List, BondingboxInfo = detector.findPosition(img)
    

    if BondingboxInfo: #바운딩 박스 정보 보내줌
        Landmark_String = '' #33개 포인트 입력
        for lm in Landmark_List:
            Landmark_String += f'{lm[0]}, {img.shape[0]-lm[1]},{lm[2]},' # y축 유니티상으로 맞춰주는 부분
        posList.append(Landmark_String)
        sock.sendto(str.encode(str(posList)), serverAddressPort)
    print(posList)
    
    
    cv2.imshow("Image", img)
    key =cv2.waitKey(1) #비디오 영상 제어하는 부분
    if key == 27 :
        break

