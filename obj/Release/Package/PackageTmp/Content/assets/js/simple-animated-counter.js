var a = 0;
$(window).scroll(function () {

    //alert($('.mu-counter-nav'));
    var nav = $('.mu-counter-nav');
    //alert(nav.length);
    if (nav.length) {
        //alert('1');
        //var oTop = $('.mu-counter-nav').offset().top - window.innerHeight;
        var oTop = nav.offset().top - window.innerHeight;
        if (a == 0 && $(window).scrollTop() > oTop) {
            $('.counter-value').each(function () {
                var $this = $(this),
                    countTo = $this.attr('data-count');
                $({
                    countNum: $this.text()
                }).animate({
                    countNum: countTo
                },

                    {

                        duration: 2000,
                        easing: 'swing',
                        step: function () {
                            $this.text(Math.floor(this.countNum));
                        },
                        complete: function () {
                            $this.text(this.countNum);
                            //alert('finished');
                        }

                    });
            });
            a = 1;
        }
    }

    

});