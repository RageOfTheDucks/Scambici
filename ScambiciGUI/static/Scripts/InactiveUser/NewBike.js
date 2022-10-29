$(document).ready(function(){
	$("#request-new-bike").submit(function(event) {
		var newBikeRequest = $('#request-new-bike').serializeObject();
		let timestamp = $.getTimestamp();
		newBikeRequest['UserId'] = $.session.get("userId");
		newBikeRequest['Signature'] = $.calculateSignature(timestamp);
		newBikeRequest['Timestamp'] = timestamp;

		if ($('#no-delivery-req').is(':checked')) {
			var d = new Date();
			newBikeRequest['DesiredDateTime'] = d.toISOString();
		}
		else {
			newBikeRequest['DesiredDateTime'] = newBikeRequest['DesiredDate'] + ' ' + newBikeRequest['DesiredTime'];
		}

		$.ajax({ 
			type: "POST",
			url: POST_NEWBIKE_URL,
			data: JSON.stringify(newBikeRequest),
			dataType: "json",
			contentType : "application/json",
			success: function() {
				alert("Richiesta inoltrata con successo.");
				window.location.href = "/Scambici/InactiveUser/HomeInactiveUser";
			},
			error: function() {
				alert("Errore durante l'invio della richiesta.");
			}

		});
		event.preventDefault();
	});
	$('input[name=delivery]', '#request-new-bike').change(function() {
		var requestDelivery = $(this).val();
		if(requestDelivery == "false") {
			$('#request-delivery').hide(200);
		}
		else if(requestDelivery == "true") {
			$('#request-delivery').show(200);
		}
	});
});	
