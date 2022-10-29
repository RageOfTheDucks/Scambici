$(document).ready(function(){
	$("#report-theft").submit(function(event) {
		var theftReport = {};
		theftReport['UserId'] = $.session.get("userId");
		let timestamp = $.getTimestamp();
		theftReport['Signature'] = $.calculateSignature(timestamp);
		theftReport['Timestamp'] = timestamp;
		theftReport['HasKeys'] = $("input[name=HasKeys][value=true]:checked").val();
		if (theftReport['HasKeys'] != true) {
			theftReport['HasKeys'] = !($("input[name=HasKeys][value=false]:checked").val());
		}

		$.ajax({ 
			type: "POST",
			url: POST_THEFTREPORT_URL,
			data: JSON.stringify(theftReport),
			dataType: "json",
			contentType : "application/json",
			success: function () {
				if (theftReport['HasKeys'] == true) {
					window.location.href = "/Scambici/ActiveUser/Confirmation/TheftReport_WithKey"
				} else {
					window.location.href = "/Scambici/ActiveUser/Confirmation/TheftReport_WithoutKey"
				}
			}, 
			error: function () {
				alert("Errore nell'invio della richiesta");
			},

		});
		event.preventDefault();
	});
});
