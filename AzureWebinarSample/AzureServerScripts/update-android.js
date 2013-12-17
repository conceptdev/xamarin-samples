function insert(item, user, request) {

	request.execute();

	// Android GCM
// request.execute({
//     success: function() {
//         // Write to the response and then send the notification in the background
//         request.respond();
//         push.gcm.send(item.channel, item.text, {
//             success: function(response) {
//                 console.log('Push notification sent: ', response);
//             }, error: function(error) {
//                 console.log('Error sending push notification: ', error);
//             }
//         });
//     }
// });
}