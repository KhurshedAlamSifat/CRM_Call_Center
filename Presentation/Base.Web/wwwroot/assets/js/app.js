jQuery(function(){
	

	jQuery('.svg_img').each(function() {
		var $img = jQuery(this);
		var imgID = $img.attr('id');
		var imgClass = $img.attr('class');
		var imgURL = $img.attr('src');
		jQuery.get(imgURL, function(data) {
		  // Get the SVG tag, ignore the rest
		  var $svg = jQuery(data).find('svg');
		  // Add replaced image's ID to the new SVG
		  if (typeof imgID !== 'undefined') {
			$svg = $svg.attr('id', imgID);
		  }
		  // Add replaced image's classes to the new SVG
		  if (typeof imgClass !== 'undefined') {
			$svg = $svg.attr('class', imgClass + ' replaced-svg');
		  }
		  // Remove any invalid XML tags as per http://validator.w3.org
		  $svg = $svg.removeAttr('xmlns:a');
		  // Check if the viewport is set, if the viewport is not set the SVG wont't scale.
		  if (!$svg.attr('viewBox') && $svg.attr('height') && $svg.attr('width')) {
			$svg.attr('viewBox', '0 0 ' + $svg.attr('height') + ' ' + $svg.attr('width'))
		  }
		  // Replace image with new SVG
		  $img.replaceWith($svg);
		}, 'xml');
	  });
	

	  $(function () {
		$(".ddl-select").each(function () {
		  $(this).hide();
		  var $select = $(this);
		  var _id = $(this).attr("id");
		  var wrapper = document.createElement("div");
		  wrapper.setAttribute("class", "ddl ddl_" + _id);
	  
		  var input = document.createElement("input");
		  input.setAttribute("type", "text");
		  input.setAttribute("class", "ddl-input");
		  input.setAttribute("id", "ddl_" + _id);
		  input.setAttribute("readonly", "readonly");
		  input.setAttribute(
			"placeholder",
			$(this)[0].options[$(this)[0].selectedIndex].innerText
		  );
	  
		  $(this).before(wrapper);
		  var $ddl = $(".ddl_" + _id);
		  $ddl.append(input);
		  $ddl.append("<div class='ddl-options ddl-options-" + _id + "'></div>");
		  var $ddl_input = $("#ddl_" + _id);
		  var $ops_list = $(".ddl-options-" + _id);
		  var $ops = $(this)[0].options;
		  for (var i = 0; i < $ops.length; i++) {
			$ops_list.append(
			  "<div data-value='" +
				$ops[i].value +
				"'>" +
				$ops[i].innerText +
				"</div>"
			);
		  }
	  
		  $ddl_input.click(function () {
			$ddl.toggleClass("active");
		  });
		  $ddl_input.blur(function () {
			$ddl.removeClass("active");
		  });
		  $ops_list.find("div").click(function () {
			$select.val($(this).data("value")).trigger("change");
			$ddl_input.val($(this).text());
			$ddl.removeClass("active");
		  });
		});
	  });
	  
	
	
	
});





















