$(document).ready(function(){
	$("#interrupt-rental").submit(function(event) {
		var rentalInterruption = {};
		rentalInterruption['UserId'] = $.session.get("userId");
		let timestamp = $.getTimestamp();
		rentalInterruption['Signature'] = $.calculateSignature(timestamp);
		rentalInterruption['Timestamp'] = timestamp;

		$.ajax({ 
			type: "POST",
			url: POST_RENTALINTERRUPT_URL,
			data: JSON.stringify(rentalInterruption),
			dataType: "json",
			contentType : "application/json",
			success: function () {
				window.location.href = "/Scambici/ActiveUser/Confirmation/RentalInterruption"
			}, 
			error: function () {
				alert("Errore nell'invio della richiesta");
			},

		});
		event.preventDefault();
	});
});	
