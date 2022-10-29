$(document).ready(function(){
	var theftReportAuth = {};
	let timestamp = $.getTimestamp();
	theftReportAuth['EmployeeId'] = $.session.get("employeeId");
	theftReportAuth['Signature'] = $.calculateSignature(timestamp);
	theftReportAuth['Timestamp'] = timestamp;

	$.ajax({ 
		type: "POST",
		url: POST_GETTHEFTREPORTREQ_URL,
		data: JSON.stringify(theftReportAuth),
		dataType: "json",
		contentType : "application/json",
		success: function(theftReports){
			if (theftReports.length === 0) {
				$("#theftReports").html('<div class="mt-5 h1 text-secondary text-center">Nessuna richiesta in sospeso</div>')
			}
			else {
				for (i in  theftReports) {
					$("#theftReports").append('<div class="container list-container bg-body text-head rounded-lg shadow"><div class="text-head my-2">Furto con chiave segnalato da<br/><span class="text-lg-head font-weight-bold">'+ theftReports[i].CreatedBy.Name +' '+ theftReports[i].CreatedBy.Surname +'</span></div><form class="theftReportsForm"><input type="hidden" name="UserId" value="'+ theftReports[i].CreatedBy.UserId +'"/><div class="mt-3"><input type="text" class="form-control" placeholder="Id bicicletta sostituita" id="BikeId" name="BikeId"/></div><div class="row"><div class="col-sm-4"><button type="submit" class="btn btn-block btn-outline-head mt-2">Non confermare</button></div><div class="col-md-8"><button type="submit" class="btn btn-block btn-head mt-2">Conferma</button></div></div></form></div>');
				}
			}
		},
		error: function(data){
			alert("Errore durante il caricamento delle richieste");
		},
	});
});

$(document).on('submit', 'form', function(event){
	var theftReportConfirmation = {};
	let timestamp = $.getTimestamp();
	theftReportConfirmation['EmployeeId'] = $.session.get("employeeId");
	theftReportConfirmation['Signature'] = $.calculateSignature(timestamp);
	theftReportConfirmation['Timestamp'] = timestamp;
	theftReportConfirmation['BikeId'] = $(this).find('input[name=BikeId]').val();
	theftReportConfirmation['UserId'] = $(this).children('input[name=UserId]').val();

	$.ajax({ 
		type: "POST",
		url: POST_THEFTREPORTCONF_URL,
		data: JSON.stringify(theftReportConfirmation),
		dataType: "json",
		contentType : "application/json",
		success: function(data) {
			alert("Segnalazione furto confermata con successo");
		},
		error: function(data) {
			alert("Errore durante la conferma segnalazione furto");
		},
	});
	event.preventDefault();
});