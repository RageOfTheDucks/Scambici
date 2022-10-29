$(document).ready(function(){
	$("#request-maintenance").submit(function(event) {
		var maintenanceRequest = {};
		maintenanceRequest['UserId'] = $.session.get("userId");
		let timestamp = $.getTimestamp();
		maintenanceRequest['Signature'] = $.calculateSignature(timestamp);
		maintenanceRequest['Timestamp'] = timestamp;
		maintenanceRequest['FaultDescription'] = $('#faultDescription').val();

		$.ajax({ 
			type: "POST",
			url: POST_MAINTENANCEREQ_URL,
			data: JSON.stringify(maintenanceRequest),
			dataType: "json",
			contentType : "application/json",
			success: function () {
				window.location.href = "/Scambici/ActiveUser/Confirmation/MaintenanceRequest"
			},
			error: function () {
				alert("Errore nell'invio della richiesta");
			},
		});
		event.preventDefault();
	});
});
