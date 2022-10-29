$(document).ready(function(){
	var deliveryAuth = {};
	let timestamp = $.getTimestamp();
	deliveryAuth['EmployeeId'] = $.session.get("employeeId");
	deliveryAuth['Signature'] = $.calculateSignature(timestamp);
	deliveryAuth['Timestamp'] = timestamp;

	$.ajax({ 
		type: "POST",
		url: POST_GETDELIVERIESREQ_URL,
		data: JSON.stringify(deliveryAuth),
		dataType: "json",
		contentType : "application/json",
		success: function(deliveries){
			if (deliveries.length === 0) {
				$("#deliveries").html('<div class="mt-5 h1 text-secondary text-center">Nessuna consegna in sospeso</div>')
			}
			else {
				for (i in  deliveries) {
					$("#deliveries").append('<div class="container list-container bg-body text-head rounded-lg shadow"><div class="text-head my-2">Consegna per <span class="text-lg-head font-weight-bold">'+ deliveries[i].CreatedBy.Name +' '+ deliveries[i].CreatedBy.Surname +'</span><br/>Data e ora: <span class="font-weight-bold">'+ deliveries[i].DesiredDateTime +'</span><br/>Indirizzo: <span class="font-weight-bold">'+ deliveries[i].CreatedBy.Address +'</span></div><form class="take-task"><input type="hidden" name="TaskId" value="'+ deliveries[i].TaskId +'"/><div class="mt-2"><button type="submit" class="btn btn-block btn-head mt-2">Prendi in carico</button></div></form></div>');
				}
			}
		},
		error: function(data){
			alert("Errore durante il caricamento delle richieste");
		},
	});
});

$(document).on('submit', 'form', function(event){
	var assignDelivery = {};
	let timestamp = $.getTimestamp();
	assignDelivery['EmployeeId'] = $.session.get("employeeId");
	assignDelivery['Signature'] = $.calculateSignature(timestamp);
	assignDelivery['Timestamp'] = timestamp;
	assignDelivery['TaskId'] = $(this).children('input[name=TaskId]').val();


	$.ajax({ 
		type: "POST",
		url: POST_ASSIGN_DELIVERY_URL,
		data: JSON.stringify(assignDelivery),
		dataType: "json",
		contentType : "application/json",
		success: function(data) {
			window.location.href = "/Scambici/Employee/HomeEmployee";
		},
		error: function(data) {
			alert("Errore durante la presa in carico della consegna");
		},
	});
	event.preventDefault();
});
