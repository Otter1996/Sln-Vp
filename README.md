# 菜鳥園藝植物網站(使用asp.net MVC framework)

一.前言:這個side project很早就開始在開始做了，中間遇到了技術上的問題，認為自己不夠懂前端，也不夠懂後端的技術觀念，所以花了不少時間找資料、釐清觀念。目前本專案還在開發中。還缺少了不少東西，羅列如下:
(一).後臺系統 
(二).OWASP
(三).signalIR(要把對話紀錄寫進資料庫)
(四).產品詳情裡的照片輪播功能
(五).申請憑證
(六).買固定IP 或者是 架在IaaS
(七).log紀錄(怎麼記?如何記?) 
(八).後臺系統的產品新增/修改/刪除
(九).後臺系統熱門商品輪播更改的功能
(十).留言板
(十一).串google fb的註冊系統api(如何做?需不需要申請?為了串api所需要的資料庫schema) 
(十二).綠界金流如要串接需要甚麼?要錢嗎?
(十三).在UserData新開一個Img欄位放大頭貼(牽扯到上傳、下載功能)
(十四).後台系統可以做出網頁點及次數的報告嗎?(折線圖)、(長條圖)

二.內容:由於前端的東西過於瑣碎，容我點出幾個重要的點出來。網站是使用Bootstrap當作框架，再整合自己所設計的幾個.css和.js功能所組合而成。而後端當然就是MVC了，由controller控制view、layout、建立與DB溝通的Model等等...。


(圖一)網站首頁，輪播圖放在navbar下，可以透過修改View更改圖片.![](https://i.imgur.com/ABM5eRR.png)
```
    <!--輪播圖-->
    <div id="carouselExampleIndicators" class="carousel slide" data-ride="carousel">
        <ol class="carousel-indicators">
            <li data-target="#carouselExampleIndicators" data-slide-to="0" class="active"></li>
            <li data-target="#carouselExampleIndicators" data-slide-to="1"></li>
            <li data-target="#carouselExampleIndicators" data-slide-to="2"></li>
        </ol>
        <div class="carousel-inner">
            <div class="carousel-item active">
                <img src="~/Images/輪播圖-1.jpg" class="d-block w-100" alt="多肉植物圖示">
                <div class="carousel-caption d-none d-md-block">
                    <h5>多肉植物</h5>
                    <p style="background-color: azure; color: maroon">外貌可愛且栽種簡易，可說是最適合小資族群的盆栽園藝。</p>
                </div>
            </div>
            <div class="carousel-item">
                <img src="~/Images/輪播圖-2.jpg" class="d-block w-100" alt="小型植栽圖示">
                <div class="carousel-caption d-none d-md-block">
                    <h5>小型植栽</h5>
                    <p style="background-color: azure; color: maroon">簡單、好照顧、能夠裝飾房間療癒身心的室內植物。</p>
                </div>
            </div>
            <div class="carousel-item">
                <img src="~/Images/輪播圖-3.jpg" class="d-block w-100" alt="大型植栽圖示">
                <div class="carousel-caption d-none d-md-block">
                    <h5>大型植栽</h5>
                    <p style="background-color:azure;color:maroon">有人說這會讓室內看起來更有層次感，更有通透性，如果室內有顆大綠植，還是很容易吸引眼球的。</p>
                </div>
            </div>
        </div>
        <a class="carousel-control-prev" href="#carouselExampleIndicators" role="button" data-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="sr-only">Previous</span>
        </a>
        <a class="carousel-control-next" href="#carouselExampleIndicators" role="button" data-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="sr-only">Next</span>
        </a>
    </div>
```
(圖二).也是使用輪播圖製作，但有透過css對圖片做簡單的動畫![](https://i.imgur.com/8HrVKg2.png)
```
.figure {
    position: relative;
    display: inline-block;
    width: 400px;
}

.figure img {
    width: 100%
}

h1 {
    position: absolute;
    left: 0;
    right: 0;
    top: 10%;
    text-align: center;
    font-family: Open Sans, sans-serif;
    opacity: 0;
    transition: all 1s ease 0.5s;/*增加文字速度*/
}

.figure:hover h1 {
    /*0變1的過程transition*/
    opacity: 1;
}

.figure .span1:before {
    content: '';
    position: absolute;
    top: 5%;
    left: 0;
    right: 0;
    width: 90%;
    height: 5px;
    background-color: transparent;
    margin: 0 auto;
}

.figure .span1:after {
    content: '';
    position: absolute;
    top: 5%;
    right: 5%;
    width: 5px;
    height: 90%;
    background-color: transparent;
    margin: 0 auto;
}

.figure .span2:before {
    content: '';
    position: absolute;
    bottom: 5%;
    left: 0;
    right: 0;
    width: 90%;
    height: 5px;
    background-color: transparent;
    margin: 0 auto;
}

.figure .span2:after {
    content: '';
    position: absolute;
    bottom: 5%;
    left: 5%;
    width: 5px;
    height: 90%;
    background-color: transparent;
    margin: 0 auto;
}

.figure:hover .span1:before {
    animation: border-top 0.3s linear;
    transform-origin: left;
    background-color: forestgreen;
}

@keyframes border-top {
    0% {
        transform: scaleX(0);
    }
}

.figure:hover .span1:after {
    animation: border-right 0.6s linear;
    transform-origin: top;
    background-color: forestgreen;
}

@keyframes border-right {
    0%, 50% {
        transform: scaleY(0);
    }
}

.figure:hover .span2:before {
    animation: border-bottom 0.9s linear;
    transform-origin: right;
    background-color: forestgreen;
}

@keyframes border-bottom {
    0%, 75% {
        transform: scaleX(0);
    }
}

.figure:hover .span2:after {
    animation: border-left 1.2s linear;
    transform-origin: bottom;
    background-color: forestgreen;
}

@keyframes border-left {
    0%, 80% {
        transform: scaleY(0);
    }
}


```

(圖三).可以透過點選電話超連結撥打skype網路電話![](https://i.imgur.com/eNXUBqJ.png)
```
<div class="col-sm-12 col-md-6 col-lg-2 col-xl-2">聯絡資訊
    <br>
    <div>電話 : <a href="tel:+886-918******">07-0000000</a></div>
    <div>傳真 : <a>暫不提供</a></div>
    <div>地址 : <a href="https://www.google.com.tw/maps/place/Kaohsiung" target="_blank">高雄市OO區OO路OO號</a></div>
    <div>E-mail : <a href="mailto: fake_email_address@gmail.com">Email-Address</a></div>
</div>
```
(圖四).點選地址打開google Map並跳出事先預訂的位址![](https://i.imgur.com/kICKTsD.jpg)
(圖五).點選email可以打開打開電子信箱![](https://i.imgur.com/P29ibAH.png)
(圖六)為會員登入介面.![](https://i.imgur.com/l1fnOPC.png)
```
@section AddToHead{
    <link href="~/Content/login.css" rel="stylesheet" />
    }
<div class="container-fluid body-content" style="margin-top: 250px">
    <div class="row">
        <div class="col">
        </div>
        <div class="col-sm-8 col-md-6 col-lg-5 col-xl-4">
            <p class="h2 text-xl-center" style="color:white">會員登入</p>
            <form class="form" method="post" action="@Url.Action("Login","Main")" novalidate id="LoginForm" autocomplete="off">
                <div class="row">
                    <div class="col-12" style="color:red" align="center">
                        @ViewBag.Message
                    </div>
                    <!--帳號-->
                    <div class="col-12">
                        <label for="Account" class="form-label">帳號</label>
                        <input type="text" class="form-control" id="Account" name="UserId" placeholder="請輸入帳號" required>
                        <div class="invalid-feedback">
                            帳號為必填。
                        </div>
                    </div>
                    <!--密碼-->
                    <div class="col-12">
                        <label for="Password" class="form-label">密碼</label>
                        <input type="text" class="form-control" id="Password" name="UserPassword" placeholder="請輸入密碼" required>
                        <div class="invalid-feedback">
                            密碼為必填。
                        </div>
                    </div>
                    <!--記住我-->
                    <div class="col-12">
                            <a href="@Url.Action("ForgetPassword","Main")" value="忘記密碼?" style="color:black">
                                忘記密碼?
                            </a>
                    </div>
                    <div class="form-group col-12" align="center">
                        <button type="submit" class="btn btn-primary" align="center">登入</button>
                    </div>
                </div>
            </form>
        </div>
        <div class="col">
        </div>
    </div>
</div>
<hr />
```
(圖七).為登入後的使用介面，會多出會員登出、購物車、歡迎光臨。其部份程式碼如下。![](https://i.imgur.com/jl7hVWP.png)
```      
/// <summary>
/// 登入機制
/// </summary>
[HttpPost]
public ActionResult Login(string UserId, string UserPassword)
{
    LoginLogic login = new LoginLogic();
    UserData LoginUser = login.IsSuccessLogin(UserId, HashFunction.Hashfun(UserPassword));
    if (LoginUser == null)
    {
        ViewBag.Message = "登入失敗，請重新輸入";
        return View("Login");
    }
    else
    {
        AuthenticationUserAndWriteIntoCookie(LoginUser);
        return RedirectToAction("Main");
    }

}

```
(圖八)全部分類裡其中大型植栽的全部商品.![](https://i.imgur.com/rxgUagd.png)
```
public ActionResult ProductIndex(string name)
{
    UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
    var Products= db.Product.Where(m => m.Pid.Contains(name)).ToList();
    return View("ProductIndex",Products);
}
```
(圖九)全部分類裡其中大型植栽的農藝用品全部商品.![](https://i.imgur.com/LsQsbIy.png)
(圖十)從(圖八)、(圖九)的商品的Detail.![](https://i.imgur.com/L38R6ii.png)
```
public ActionResult ReadDetail(string Pid)
{
    UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
    try
    {
        var Product = db.Product.Where(m => m.Pid.Contains(Pid)).FirstOrDefault();
        ViewBag.Message = Product.ProductDescription.Detail;
        /*string path = @"C:\MVC\slnVP\VegetablePlatform\Txt\" + Product.Detail;
        using (StreamReader sr = new StreamReader(path))
        {
            string line = "";
            line = sr.ReadToEnd();
            ViewBag.Message = line;
            return View("ProductDetail",Product);
        }*/
        return View("ProductDetail", Product);
    }
    catch (Exception ex)
    {
        string expection = "找不到此產品，或此產品已經下架";
        ViewBag.Message = expection;
        return View("ProductDetail", "_Layout");
    }
}
```
(圖十一)使用Bootstrap手風琴來製作介面.![](https://i.imgur.com/GBgK35C.png)
```
<div class="card">
    <div class="card-header" id="headingTwo">
        <h2 class="mb-0">
            <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                能不能教我怎麼養植物?
            </button>
        </h2>
    </div>
    <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample">
        <div class="card-body">
            能!請看以下教學。
            <div class="video-container">
            <iframe width="560" height="315" src="https://www.youtube.com/embed/PMuiaYawDoQ" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
            </div>
        </div>
    </div>
</div>
```

(圖十二).使用google map api與前端串接![](https://i.imgur.com/SlazgAf.png)
![](https://i.imgur.com/PWjHJ3V.png)
![](https://i.imgur.com/vHYlDOf.png)

```
var markers = [];
var infowindows = [];
var select_map;

function initMap(travelMode) {
    // 載入路線服務與路線顯示圖層
    var directionsService = new google.maps.DirectionsService();
    var directionsDisplay = new google.maps.DirectionsRenderer();


    // 初始化地圖
    select_map = new google.maps.Map(document.getElementById('map'), {
        zoom: 10,
        center: { lat: 22.683366691184965, lng: 120.29669086057828 }
    });
    // 放置路線圖層
    directionsDisplay.setMap(select_map);

    // 路線相關設定
    var request = {
        origin: { lat: 22.687916655554677, lng: 120.30725314748948 },
        destination: { lat: 22.683366691184965, lng: 120.29669086057828 },
        travelMode: travelMode,
    };

    // 繪製路線
    directionsService.route(request, function (result, status) {
        if (status == 'OK') {
            var steps = result.routes[0].legs[0].steps;
            steps.forEach((e, i) => {
                console.log(steps);
                //加入地圖記號
                markers[i] = new google.maps.Marker({
                    position: { lat: e.start_location.lat(), lng: e.start_location.lng() },
                    map: select_map,
                    label: { text: i + '', color: "#fff" }
                });
                //加入資訊視窗
                infowindows[i] = new google.maps.InfoWindow({
                    content: e.instructions
                });
                //加入地圖標記點擊事件
                markers[i].addListener('click', function () {
                    //是否展開
                    if (infowindows[i].anchor) {
                        //關閉
                        infowindows[i].close();
                    } else {
                        //展開
                        infowindows[i].open(map, markers[i]);
                    }
                });
            });
            directionsDisplay.setDirections(result);
        } else {
            console.log(status);
        }
    });

}

$("#transit_header").click(function () {
    initMap('TRANSIT');//啟動API
});
$("#drive_header").click(function () {
    initMap('DRIVING');//啟動API
});
$("#walk_header").click(function () {
    initMap('WALKING');//啟動API
});
```
(圖十三).點選數位客服後跳出及時對話新視窗(尚未完成signalR部分)![](https://i.imgur.com/kG5Qawl.png)
(圖十四).忘記密碼寄出email輸入驗證碼方可設定新密碼![](https://i.imgur.com/SOEoDJs.jpg)![](https://i.imgur.com/YNF2DWK.jpg)
```
/// <summary>
/// 忘記密碼之Email驗證碼功能
/// </summary>
/// <param name="post">Email</param>
[HttpPost]
[MultiButton("Verify")]
public ActionResult SendEmail(FormCollection post)
{

    string Email = post["Email"];
    string subject = "此為系統自動發送信件，請勿直接回覆。";

    Random VerifyNumber = new Random();

    UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();

    var member = db.UserActivation
        .Where(m => m.email == Email).FirstOrDefault();

    if (member != null)
    {
        member.Random = Convert.ToString(VerifyNumber.Next(99999));
        db.SaveChanges();
        string body = "親愛的顧客您好，以下為您的Email驗證碼:【 " + member.Random + " 】，請在10分鐘內前往填寫驗證碼。";
        WebMail.Send(Email, subject, body, null, null, null, true, null, null, null, null, null);
        ViewBag.msg = "Email成功傳送，請至信箱擷取驗證碼...";
        return View("ForgetPassword");
    }
    else
    {
        ViewBag.msg = "Email傳送失敗，請聯絡管理員進行了解(開玩笑的不要找我)";
        return View("ForgetPassword");
    }

}


---
[HttpPost]
[MultiButton("Confirm")]
public ActionResult Confirm(FormCollection post)
{
    UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
    string email = post["Email"];
    string account = post["Account"];
    string newpassword = post["Password"];

    var member = db.UserData
        .Where(m => m.account == account && m.email == email).FirstOrDefault();
    if (member != null)
    {
        if (post["Verify"] == member.UserActivation.Random)
        {
            member.password = HashFunction.Hashfun(newpassword);
            member.UserActivation.Random = null;
            db.SaveChanges();

            ViewBag.ChangePassword = "更改密碼成功!";
            return View();
        }
        else
        {
            ViewBag.ChangePassword = "輸入驗證碼錯誤，請重新傳送驗證碼。";
            member.UserActivation.Random = null;
            return View();
        }

    }
    else
    {
        ViewBag.ChangePassword = "找不到此用戶，或輸入Email不相符，請重新再試。";
        return View();
    }
}
```

(圖十五).使用Bootstrap網格系統所做出的排版(手機、平板、電腦螢幕皆可用)![](https://i.imgur.com/Nb8bcGf.png)

(圖十六).navbar也沒問題!![](https://i.imgur.com/ivMTCuq.png)

(圖十七).會員註冊介面![](https://i.imgur.com/JIkndoF.png)![](https://i.imgur.com/EazXhTS.png)

(圖十八)網頁內建的搜尋引擎，能尋找資料庫裡的商品.![](https://i.imgur.com/KbRlbrF.png)
```
[HttpGet]
public ActionResult Search(string productname)
{
    UserDataBaseEntitiesEntities db = new UserDataBaseEntitiesEntities();
    var product = (from x in db.Product where x.Name.Contains(productname) select x);
    var productlist = product.ToList();
    return View("SearchResult", productlist);
}
--------------------------------------------------
let name = document.getElementById('productname');
document.getElementById("btn-search").addEventListener('click', function (){
    if (name.value.length > 10) {
        alert('不可以輸入超過10個字，請重新搜尋。');
        name.value = '';
        event.preventDefault();
    }
    else if (name.value.length < 2)
    {
        alert('不可以輸入少於2個字，請重新搜尋。');
        name.value = '';
        event.preventDefault();
    }
});
```

參考資料:
1.跟著實務學習asp.net MVC 5.x
2.深入淺出C#
3.CLR via C#
4.資訊安全概論與實務
5.https://bootstrap.hexschool.com/docs/4.2/getting-started/introduction/
6.https://developer.mozilla.org/zh-TW/docs/Web/Tutorials
7.https://developers.google.com/maps/documentation/javascript/overview





















