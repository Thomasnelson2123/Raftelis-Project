function sortTable(f,n, colId){
	var rows = $('#Parcels tbody  tr').get();
	if (colId == "ADDRESS") {
		rows.sort(function(a,b) {
			var A = getAddressVal(a);
			var B = getAddressVal(b);
			var nameCompare = compare(A[0], B[0]);
			if (nameCompare == 0) {
				return compare(A[1], B[1]);
			}
			else {
				return nameCompare;
			}

		});
	}
	else if (colId == "OWNER") {
		rows.sort(function(a,b) {
			var A = getOwnerVal(a);
			var B = getOwnerVal(b);
			return compare(A,B);
		});
	}
	else if (colId == "SALE_DATE") {
		rows.sort(function(a,b) {
			var A = getDateVal(a);
			var B = getDateVal(b);
			return compare(A,B);
		});
	}
	else {
		rows.sort(function(a, b) {
			var A = getVal(a);
			var B = getVal(b);
			return compare(A, B);
		});
	}

	// takes a row (elm) and returns the specified value of column n in that row
	function getVal(elm){
		var v = $(elm).children('td').eq(n).text().toUpperCase();
		if($.isNumeric(v)){
			v = parseInt(v,10);
		}
		return v;
	}

	function compare(A, B) {
		if(A < B) {
			return -1*f;
		}
		if(A > B) {
			return 1*f;
		}
		return 0;
	}

	// modified version of getVal used to grab the address. Splits it into street name and number
	function getAddressVal(elm) {
		var address = $(elm).children('td').eq(n).text().trim().toUpperCase();
		// can add additional delimiters here if additional data calls for it
		var regex = /\s*-\s*|\s+/;
		var splitAddress = address.split(regex);
		var streetName = splitAddress.slice(1).join(' ');
		var streetNumber = parseInt(splitAddress[0], 10);
		return [streetName, streetNumber];
	}

	function getOwnerVal(elm) {
		var owner = $(elm).children('td').eq(n).text().trim().toUpperCase();
		var splitName = owner.split(',');
		if (splitName.length > 1) {
			return splitName.slice(1).join(' ').trim();
		}
		else {
			return splitName[0];
		}
	}

	function getDateVal(elm) {
		var date = $(elm).children('td').eq(n).text().trim().toUpperCase();
		var parts = date.split('/');
		return new Date(parts[2], parts[0] - 1, parts[1]).getTime();
	}

	$.each(rows, function(index, row) {
		$('#Parcels').children('tbody').append(row);
	});
}
$(function() {
    var headers = document.querySelectorAll("#Parcels th");
    var headerMap = {};
    headers.forEach(function(header) {
        headerMap[header.id] = 1;
    });
    $(".clickable").on("click", function(){    
        var colId = $(this).attr("id");
        headerMap[colId] *= -1;
		// get column index
        var n = $(this).prevAll().length;
        sortTable(headerMap[colId],n, colId);
    });
    
});