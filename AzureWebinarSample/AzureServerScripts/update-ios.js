function update(item, user, request)
{
    request.execute();

    // iOS APNS
//    setTimeout(function() {
//        push.apns.send(item.DeviceToken, {
//            alert: "Push! " + item.text,
//            payload: {
//                inAppMessage: "Yup, this got updated: '" + item.text + "'"
//            }
//        });
//    }, 2500);
}