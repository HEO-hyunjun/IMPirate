# IMPirate
python mediapipe, opencv를 활용하여 카메라로 사람을 인식하여 그 사람의 관절을 UDP통신을 이용해 Unity로 받아와서 포즈를 인식하여 게임의 입력으로 활용합니다.
그리고 Mirror 오픈소스를 활용하여 간이 P2P서버를 구축하여 다른 사람들과 실시간으로 경쟁할 수 있도록 했습니다.

## 개발환경
사용언어 : C#, Python<br>
협업 : flow (https://flow.team/), discord<br>
형상관리 : git<br>
Unity version : 2022.3.8f1<br>

## 제작기간
2023/11/29 ~ 2023/12/16

## 팀원
노순혁 : 카메라를 통한 포즈인식 이후 UDP통신을 활용해 Unity에 전송<br>
신해인 : Mirror를 활용해 멀티 플레이어 구현<br>
허현준 : 게임 기획과 플레이어, 아이템, 게임플레이 씬 UI, 맵디자인, 애니메이션, 상호작용 구현

## 게임 규칙
1. 게임은 총 10분 진행합니다.
2. 점수를 가장 많이 획득한 사람이 승리합니다.
3. 점수는 다음과 같은 경우 획득합니다.
- (사용) 아이템을 획득할때 5점
- 동전 아이템을 획득할때 10점
- 다른 플레이어를 죽였을때 150점
- 살아있다면, 1초에 1점

## 인식 가능한 포즈
1. 왼팔들기, 오른팔들기 : 어깨의 각도에 따라 얼마만큼 들었는지 -> 좌 우 Axis값
2. 박수치기 : 확인 / 공격
3. 손으로 X 만들기 : 취소 / 후진
4. (인게임) 왼쪽팔 높이들기, 오른쪽팔 높이들기 : 아이템1, 2 사용

## 아이템 설명
1. 획득아이템 <br>
플레이어가 아이템을 획득하자마자 영구적인 효과를 얻습니다.
- 탄환 <br>탄환을 5~10개 획득합니다.
- 동전 <br>점수를 즉시 10~20 획득합니다.
- 신속날개<br>속도 레벨을 1~2 증가시킵니다.
- 대포강화<br>공격력을 5~10 증가시킵니다.

2. 사용아이템 <br>
플레이어가 아이템을 획득하고 사용할때까지 아이템슬롯에 저장되는 아이템이며, 효과는 일시적입니다.
- 임시날개<br>15초간 속도 레벨을 2~3 증가시킵니다.
- 수리키트<br>5초간 체력을 20~30 회복합니다.
- 불대포<br>20초간 공격력을 2~3배 증가시킵니다.
- 공격방어<br>15초간 어떠한 피해도 입지 않습니다.
- 빠른공격<br>공격 속도가 2~3배 빨라집니다.
- 지뢰 <br>밟으면 20의 피해를 입는 지뢰를 설치합니다.

## 실행
- python 환경설정
    1. conda 설치
    2. 프로젝트 폴더 접근
    3. `conda env create -f conda_requirements.yaml` 입력 <br>새로운 가상환경에 라이브러리 설치 
    4. `conda activate IMPirate` 입력 <br>가상환경 실행
    5. `python MediaPipe/MotionCap.py` 입력 <br>모션캡쳐 프로그램 실행, esc입력시 종료 (선택)
- IMPirate.exe 실행
## 모션캡처 프로그램 미사용시 입력 키
1. 좌, 우 : a,d / 화살표 키
2. 확인/공격 : z
3. 취소/후진  : c
4. 아이템사용 1, 2 : q, w


## 추가 라이선스
1. 유니티 에셋(유료)<br>
Feel<br>
PolygonParticleFX<br>
PolygonPirates<br>
SimpleFX<br>
Cute UI _ Interact Sound Effects Pack<br>
Game Music Stingers and UI SFX Pack 2
2. 애니메이션<br>
Mixamo.com
3. 폰트<br>
던파 연단된 칼날(https://df.nexon.com/df/pg/dnfforgedblade)<br>
프리텐다드(https://github.com/orioncactus/pretendard/blob/main/LICENSE)<br>
4. 아이콘<br>
Generic Detailed Outline : FreePik- kerismaker (https://kr.freepik.com/icon/pirate_7893774#fromView=resource_detail&position=10)<br>
Icons created by Lorc, Delapouite & contributors (https://game-icons.net/) licensed under CC BY 3.0.
5. 음성<br>
이 콘텐츠는 인공지능 가상 연기자 서비스, 타입캐스트를 활용하여 제작되었습니다. (https://typecast.ai/kr)
사용 음성캐릭터 : 프랑켄슈타인
