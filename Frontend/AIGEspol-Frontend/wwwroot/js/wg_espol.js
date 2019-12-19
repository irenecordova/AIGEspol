$( document ).ready(function() {
    $.fn.sistemasEspol = function(){
           console.log("entre");
		var dominio = "https://www.profesor.espol.edu.ec/";
		//default icons settings:
	    var icon_top_row = {
	        profesores: {
	        	icon: dominio + "Content/Images/portal_min/profesor_min.png",
	        	url: "https://www.profesor.espol.edu.ec",
	        	text: "Profesor",
	        	color: "#F5b400"
	        },
	        consejerias: {
	        	icon: dominio + "Content/Images/portal_min/consejerias_min.png",
	        	url: "http://consejerias.espol.edu.ec",
	        	text: "Consejerías",
	        	color: "#25223f"
	        },
	        investigacion: {
	        	icon: dominio + "Content/Images/portal_min/investigacion_min.png",
	        	url: "http://investigacion.espol.edu.ec/Account/LogOn",
	        	text: "Investigación",
	        	color: "#808080"
	        },
	       
	    };

	    var icon_media_row = {

	        sidweb: {
	            icon: dominio + "Content/Images/portal_min/sidweb_min.png",
	            url: "https://sidweb.espol.edu.ec",
	            text: "Sidweb",
	            color: "#d2d2d2"
	        },
	       
	        vinculos: {
	            icon: dominio + "Content/Images/portal_min/vinculos_min.png",
	            url: "http://www.vinculos.espol.edu.ec/",
	            text: "Vínculos",
	            color: "#F5b400"

	        },
	        evaluacion_docente: {
	            icon: dominio + "Content/Images/portal_min/evaluacion_min.png",
	            url: "http://evaluaciondocente.espol.edu.ec/inicio",
	            text: "Evaluación",
	            color: "#25223f"

	        },
	    };
	    var icon_bottom_row = {
	        correo: {
	        	icon: dominio + "Content/Images/portal_min/correo_min.png",
	        	url: "https://mail.espol.edu.ec",
	        	text: "Correo",
	        	color: "#808080"
	        },
	        gestion_cursos: {
	        	icon: dominio + "Content/Images/portal_min/gestion_min.png",
	        	url: "http://gestioncurso.espol.edu.ec",
	        	text: "Gestión de Cursos",
	        	color: "#d2d2d2"
	        },
	        talento_humano: {
	        	icon: dominio + "Content/Images/portal_min/talento_min.png",
	        	url: "http://sth.talentohumano.espol.edu.ec/login.aspx",
	        	text: "Talento Humano",
	            color: "#F5b400"
	        },
	        
	    };

	    return this.each(function() {
	    	var widget = $("<div class='wg_espol_dropdown_login wg_espol_dropdown_menu wg_espol_dropdown_menu_right'></div>");
	    	widget.append( "<div class='wg_espol_triang_sup'></div>" );

	    	var top_row = $("<div class='row wg_espol_body_portal_imagen_min'></div>");
	    	$.each( icon_top_row, function( i, item ) {
	    	    var html_a = "<div class='wg_espol_block_portal'><a class='wg_a' style='padding: 0 !important;' href='" + item.url + "' title='Cick para acceder al sistema de " + item.text + "'>" +
                    			"<img src=" + item.icon + " alt=" + item.text + " class='block-portal-imagen' style='border-color:" + item.color + ";'>" + "</a></div>";
                top_row.append(html_a);
	    	});

	    	//agregamos la primera fila al contenedor
	    	widget.append(top_row);


	    	var media_row = $("<div class='row wg_espol_body_portal_imagen_min'></div>");
	    	$.each(icon_media_row, function (i, item) {
	    	    var html_a = "<div class='wg_espol_block_portal'><a class='wg_a' style='padding: 0 !important;' href='" + item.url + "' title='Cick para acceder al sistema de " + item.text + "'>" +
                    			"<img src=" + item.icon + " alt=" + item.text + " class='block-portal-imagen' style='border-color:" + item.color + ";'>" + "</a></div>";
	    	    media_row.append(html_a);
	    	});

	        //agregamos la primera fila al contenedor
	    	widget.append(media_row);

	    	var bottom_row = $("<div class='row wg_espol_body_portal_imagen_min'></div>");
	    	$.each(icon_bottom_row, function (i, item) {
	    	    var html_a = "<div class='wg_espol_block_portal'><a class='wg_a' style='padding: 0 !important;' href='" + item.url + "' title='Cick para acceder al sistema de " + item.text + "'>" +
                    			"<img src=" + item.icon + " alt=" + item.text + " class='block-portal-imagen' style='border-color:" + item.color + ";'>" + "</a></div>";
	    	    bottom_row.append(html_a);
	    	});

	        //agregamos la primera fila al contenedor
	    	widget.append(bottom_row);


	    	//agreamos una clase al elemento A
	        $(this).addClass("wg_espol_icon_main");
	        //agregamos el evento click para que muestre los menus
	        $(this).click(function(){
	        	$(this).parent().toggleClass("wg_espol_open")
	        })

	        //agregamos el widget despues del elemento A
	        widget.insertAfter($(this));
		});
	};
    $.fn.sistemasEspol2 = function (menus) {
        var menu_items = [];

        for (i = 0; i < (menus.length); i++)
        {
            if(menus[i] == "ayudantia"){
                menu_items.push({
                     LinkText: "Ayudantias",
                     Link: "http://www.ayudantias.espol.edu.ec",
                     LinkClass: "img-ayudantias",
                });
            }
            else if(menus[i] == "cenacad"){
                menu_items.push({
                     LinkText: "Cenacad",
                     Link:"https://www.cenacad.espol.edu.ec/",
                     LinkClass:  "img-cenacad"
                });
            }
            else if(menus[i] == "academico"){
                menu_items.push({
                     LinkText: "Académico",
                     Link: "https://www.academico.espol.edu.ec",
                     LinkClass: "img-academico",
                });
            }
            //else if(menus[i] == "solicitudes"){
            //    menu_items.push({
            //         LinkText: "Solicitudes",
            //         Link: "https://sidweb.espol.edu.ec/",
            //         LinkClass: "img-solicitudes",
            //    });
            //}
            else if(menus[i] == "certificados"){
                menu_items.push({
                    LinkText: "Certificados y Solicitudes",
                     Link: "http://www.certificados.espol.edu.ec/",
                     LinkClass: "img-certificados",
                });
            }
            else if(menus[i] == "repositorio"){
                menu_items.push({
                     LinkText: "Repositorio",
                     Link: "https://www.dspace.espol.edu.ec/",
                     LinkClass: "img-repositorio",
                });
            }
            else if(menus[i] == "vinculos"){
                menu_items.push({
                     LinkText: "Vínculos",
                     Link: "http://www.vinculos.espol.edu.ec/",
                     LinkClass: "img-vinculos",
                });
            }
        }

        
        var index = 0;
        var index_color = 0;

        var num_items = menu_items.length;
        var max_items = 10;

        var item_fila = 1;
        var max_item_fila = 1;
        var restar_max_item_fila = [0, 0, 0, 0];

        var banner_block_big = "";
        var login_block_big = "";

        if (num_items <= 6) {
            banner_block_big = "banner-block-big";
            login_block_big = "style='min-width: 270px;'";
        }

        var widget = $("<div class='wg_espol_dropdown_login wg_espol_dropdown_menu wg_espol_dropdown_menu_right' "+login_block_big+"></div>");
        widget.append("<div class='wg_espol_triang_sup'></div>");

        var menus_color = [
            "bg-gris","bg-amarillo", "bg-azul","bg-azul","bg-amarillo","bg-gris","bg-gris","bg-azul", "bg-amarillo","bg-azul"
        ];

        for (i = 0; i < (max_items - num_items); i++)
        {
            var j = i;
            if (j >= 4) {
                j = 0;
            }
            restar_max_item_fila[j]++;
        }

        for (var i = 0; i < 4; i++) {
            if (i == 0) {
                item_fila = 1;
                max_item_fila = 1 - restar_max_item_fila[3];
            }
            else if (i == 1) {
                item_fila = 2;
                max_item_fila = 2 - restar_max_item_fila[2];
            }
            else if (i == 2) {
                item_fila = 3;
                max_item_fila = 3 - restar_max_item_fila[1];
            }
            else if (i == 3) {
                item_fila = 4;
                max_item_fila = 4 - restar_max_item_fila[0];
            }

            var menu = $("<div class='block-row clearfix'></div>");

            for (var j = 0; j < item_fila; j++) {
                if (max_item_fila > j) {
                    var submenu = menu_items[index];
                    console.log(submenu);
                    menu.append('<a  class="wg_a banner-block pull-left img-ico ' + menus_color[index_color] + ' ' + submenu.LinkClass + ' ' + banner_block_big + '" style="padding: 0 !important;" href="' + submenu.Link + '" title="Cick para acceder al sistema de ' + submenu.LinkText + '"></a>');
                    index++;
                }
                index_color++;
            }
            //agregamos la primera fila al contenedor
            widget.append(menu);
        }

        //agreamos una clase al elemento A
        $(this).addClass("wg_espol_icon_main");
        //agregamos el evento click para que muestre los menus
        $(this).click(function () {
            $(this).parent().toggleClass("wg_espol_open")
        })

        //agregamos el widget despues del elemento A
        widget.insertAfter($(this));
    };
    $.fn.menuSistemasEspol = function (menus) {
        var menus_color = [
            "bg-gris", "bg-amarillo", "bg-azul", "bg-azul", "bg-amarillo", "bg-gris", "bg-gris", "bg-azul", "bg-amarillo", "bg-azul"
        ];
        var index = 0;
        var index_color = 0;

        var num_items = menus.length;
        var max_items = 10;

        var item_fila = 1;
        var max_item_fila = 1;
        var restar_max_item_fila = [0, 0, 0, 0];

        var banner_block_big = "";

        if (num_items <= 6) {
            banner_block_big = "banner-block-big";
        }

        for (i = 0; i < (max_items - num_items) ; i++) {
            var j = i;
            if (j >= 4) {
                j = 0;
            }
            restar_max_item_fila[j]++;
        }

        for (var i = 0; i < 4; i++) {
            if (i == 0) {
                item_fila = 1;
                max_item_fila = 1 - restar_max_item_fila[3];
            }
            else if (i == 1) {
                item_fila = 2;
                max_item_fila = 2 - restar_max_item_fila[2];
            }
            else if (i == 2) {
                item_fila = 3;
                max_item_fila = 3 - restar_max_item_fila[1];
            }
            else if (i == 3) {
                item_fila = 4;
                max_item_fila = 4 - restar_max_item_fila[0];
            }

            var menu = $("<div class='block-row clearfix'></div>");
            for (var j = 0; j < item_fila; j++) {
                if (max_item_fila > j) {
                    console.log(menus[index]);
                    var submenu = menus[index];
                    menu.append('<a style="left: 1140px;opacity: 0;" class="banner-block pull-left img-ico ' + menus_color[index_color] + ' ' + submenu.LinkClass + ' ' + banner_block_big + '" href="' + submenu.Link + '" title="' + submenu.LinkText + '"><span>' + submenu.LinkText + '</span></a>');
                    index++;
                }
                index_color++;
            }
            $(this).append(menu);

            //Efecto
            var duracion = 250;
            $(".img-ico").each(function (i) {
                var e = $(this);
                setTimeout(function () {
                    e.animate({ 'left': '0px', 'opacity': '1' }, { duration: duracion });
                }, getRandomInt(1, num_items) * 150);
                duracion += 100;
            });
        }
    };
    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    };
});