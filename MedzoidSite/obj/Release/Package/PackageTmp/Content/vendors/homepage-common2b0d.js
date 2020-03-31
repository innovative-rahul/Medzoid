const SERVER_BASE_URL = "/",
    BLOG_SERVER_BASE_URL = "/blog/",
    CONST_WORK_FILTER = "work_filter",
    HUB_SPOT_URL = "https://api.hsforms.com/submissions/v3/integration/submit/3775710/329369a9-dc11-4137-a1ae-52b3aa419d51",
    HUB_SPOT_KEY = "3775710",
   
    HOME_PAGE_NAME = "Best UI/UX Design studio from Hyderabad, India for enterprises and startups",
    ABOUT_PAGE_NAME = "Award winning UI/UX Agency based out of Hyderabad, India",
    PORTFOLIO_PAGE_NAME = "Portfolio: Product, Responsive Web and Mobile Applications",
    PEOPLE_PAGE_NAME = "Divami Team : The Best UI/UX Designers and Developers | QA Engineers",
    PROCESS_PAGE_NAME = "Divami : Agile UI/UX Design Process | Customer Experiences",
    UX_DESIGNER_MAIL_SUBJECT = "Regarding my application for the role - UX Designer",
    UI_DESIGNER_MAIL_SUBJECT = "Regarding my application for the role - UI Designer",
    UX_DEVELOPER_MAIL_SUBJECT = "Regarding my application for the role - UX Developer",
    QA_ENGINEER_MAIL_SUBJECT = "Regarding my application for the role - QA Engineer",
    HR_EMAIL_ID = "hr@divami.com",
    EMAIL_BODY = "Hi Vidya,",
    TAILOR_BLOCK_FIRST_TITLE = "Simplifying enterprise UX",
    TAILOR_BLOCK_SECOND_TITLE = "Digitally transforming logistics",
    TAILOR_BLOCK_THIRD_TITLE = "Going rural with TV & OTT services",
    TAILOR_BLOCK_FOURTH_TITLE = "Crypto for the common man",
    TAILOR_BLOCK_FIFTH_TITLE = "Helping doctors save lives",
    TAILOR_BLOCK_SIXTH_TITLE = "1M+ downloads and counting...",
    TAILOR_BLOCK_SEVENTH_TITLE = "Unifying payments, wallets & cards",
    TAILOR_BLOCK_EIGTH_TITLE = "Helping eradicate illiteracy",
    READ_OUR_CASE_STUDY = "Read our case ctudy",
    SEE_OUR_PORTFOLIO = "See our portfolio",
    OUR_DESIGN_PROCESS = "Our design process",
    OUR_SERVICES = "Our services",
    READ_MORE = "Read More",
    TRUTH_LIE_WRONG_MESSAGE = "Oh no! You were so close... Here's the lie!",
    ABOUT_FOOTER_CONTENT = "Where there is excellence, explanation is not required. But we did it anyways for SEO.",
    ABOUT_MESSAGE_LABEL = "Let’s create an excellent product together. Share your idea.",
    PEOPLE_FOOTER_CONTENT = "We steal the hearts of your users so we deserve ransoms, not fees!",
    PEOPLE_FOOTER_LABEL = "Your Idea that is worth a ransom?",
    PROCESS_FOOTER_CONTENT = "Our big ears are waiting to listen to your ideas.",
    PROCESS_FOOTER_LABEL = "All ears...",
    PORTFOLIO_FOOTER_CONTENT = "Let's barter. Your challenges - our solutions!",
    PORTFOLIO_FOOTER_LABEL = "Challenge us with your challenge!",
    SORRY_TRY_AGAIN = "Sorry, Try again",
    THANK_YOU_FOR_SUBMISSION = "Thank you for your submission.",
    PLEASE_FILL_ALL_THE_FIELDS = "Please fill all the required details correctly.",
    MOBILE = "mobile",
    RESPONSIVE = "web",
    TV = "tv",
    WR = "wr",
    GA_CLUTCH = "Clutch",
    GA_FACEBOOK = "Facebook",
    GA_INSTAGRAM = "Instagram",
    GA_LINKEDIN = "LinkedIn",
    GA_GLASSDOOR = "Glassdoor",
    GA_ADDRESS = "Address",
    GA_FORM_SUBMISSION = "Form submission",
    GA_HOME_PAGE_NAME = "Home page",
    GA_OUR_IMPRESSIONS = "Our impressions",
    GA_VIEW_CUSTOMERS = "View customers",
    GA_MOBILE_APPS_MAP = "Mobile Apps in Maps section",
    GA_WEB_APPS_MAP = "Web Apps in Maps section",
    GA_WEARABLE_APPS_MAP = "Wearable Apps in Maps section",
    GA_TV_APPS_MAP = "Tv Apps in Maps section",
    GA_SEE_OUR_PORTFOLIO = "See Our Portfolio",
    GA_OUR_DESIGN_PROCESS = "Our Design Process",
    GA_READ_MORE = "Read More",
    CLUTCH_URL = "https://clutch.co/profile/divami-design-labs",
    FACEBOOK_URL = "https://www.facebook.com/DivamiDesignLabs/",
    LINKDIN_URL = "https://www.linkedin.com/company/divami-design-labs/",
    GLASSDOOR_URL = "https://www.glassdoor.co.in/Overview/Working-at-Divami-EI_IE953753.11,17.htm",
    INSTAGRAM_URL = "https://www.instagram.com/divami.design/?hl=en";

function isScrolledIntoView(elem) {
    var docViewTop = $(window).scrollTop(),
        docViewBottom = docViewTop + $(window).height(),
        elemTop = $(elem).offset().top;
    return docViewTop + docViewBottom - (elemTop + (elemTop + $(elem).height())) + 500 > 1
}

function disableHTMLScroll() {
    $("html").css("overflow", "hidden")
}

function enableHTMLScroll() {
    $("html").css("overflow", "auto")
}

function loadTemplatesThroughNunjucks(element, template, data) {
    nunjucks.configure("_partials", {
        autoescape: !0,
        web: {
            async: !0
        }
    }), nunjucks.render(template, data, function(err, res) {
        $(element).html(res)
    })
}

function loadTemplatesThroughNunjucks(element, template, data, callback) {
    nunjucks.configure("_partials", {
        autoescape: !0,
        web: {
            async: !0
        }
    }), nunjucks.render(template, data, function(err, res) {
        $(element).html(res), callback && callback()
    })
}

function getElementView(element, fullyInView) {
    var pageTop = $(window).scrollTop(),
        pageBottom = pageTop + $(window).height(),
        elementTop = $(element).offset().top,
        elementBottom = elementTop + $(element).height();
    return !0 === fullyInView ? pageTop < elementTop && pageBottom > elementBottom : elementTop <= pageBottom && elementBottom >= pageTop
}

function bindFunctionToScroll(functionTobeCalled) {
    $(window).scroll(functionTobeCalled)
}

function setMasonry(setupOptions) {
    var masonry = $(setupOptions.elementClass).masonry(setupOptions.options);
    return setupOptions.callback && masonry.one("layoutComplete", function() {
        setupOptions.callback()
    }), setupOptions.data && masonry.append(setupOptions.data).masonry("appended", setupOptions.data), masonry
}

function setMasonryOptions(elementClass, options) {
    return $(elementClass).masonry(options)
}

function getLocalStorage(key) {
    return localStorage.getItem(key)
}

function setLocalStorage(key, value) {
    localStorage.setItem(key, value)
}

function removeLocalStorage(key) {
    localStorage.removeItem(key)
}

function clearLocalStorage() {
    localStorage.clear()
}

function doAjaxCallForForm(callType, url, data, callHeaders, successCallback, errorCallback, completeCallback) {
    $.ajax(url, {
        type: callType,
        data: JSON.stringify({
            fields: data,
            context: {
                pageUri: window.location.href
            }
        }),
        dataType: "json",
        headers: callHeaders,
        success: function(response) {
            successCallback()
        },
        error: function(response) {
            errorCallback()
        },
        complete: function(response) {
            completeCallback()
        }
    })
}

function gotoTop(top) {
    $("html, body").animate({
        scrollTop: top
    }, 500)
}

function hideLoader() {
    $(".loader").hide()
}

function disableBackgroundScroll(id) {
    var overlay, _clientY = null;
    (overlay = document.getElementById(id) ? document.getElementById(id) : null).addEventListener("touchstart", function(event) {
        1 === event.targetTouches.length && (_clientY = event.targetTouches[0].clientY)
    }, !1), overlay.addEventListener("touchmove", function(event) {
        1 === event.targetTouches.length && function(event) {
            var clientY = event.targetTouches[0].clientY - _clientY;
            0 === overlay.scrollTop && clientY > 0 && event.preventDefault();
            overlay.scrollHeight - overlay.scrollTop <= overlay.clientHeight && clientY < 0 && event.preventDefault()
        }(event)
    }, !1)
}

function google_analytics_tracking(eventCategory, eventLabel, eventValue) {
    gtag("event", "click", {
        event_category: eventCategory,
        event_label: eventLabel,
        value: eventValue
    })
}

function lazyLoadImage() {
    const lazyImage = document.getElementsByClassName("lazy");
    for (let i = 0; i < lazyImage.length; i++)
        if (!lazyImage[i].getAttribute("class").includes("added")) {
            findPos(lazyImage[i]) < document.documentElement.scrollTop + window.innerHeight + 100 && (lazyImage[i].setAttribute("src", lazyImage[i].getAttribute("data-src")), lazyImage[i].setAttribute("class", lazyImage[i].getAttribute("class") + " added"))
        }
}

function findPos(ele) {
    var node = ele,
        curtop = 0,
        curtopscroll = 0;
    if (node.offsetParent) {
        do {
            curtop += node.offsetTop, curtopscroll += node.offsetParent ? node.offsetParent.scrollTop : 0
        } while (node = node.offsetParent);
        return curtop - curtopscroll
    }
}

function setSlider(element, options, progressBar, progressBarLabel, isSetArrowsVisibilities) {
    (progressBar || progressBarLabel) && setProgressBarMethods(element, progressBar, progressBarLabel), $(element).slick(options), isSetArrowsVisibilities && setArrowShowHideMethods(element, options.prevArrow, options.nextArrow)
}

function setSliderWithOutNavbars(element, options, callback) {
    callback && $(element).on("init", function(event, slick, currentSlide) {
        callback()
    }), $(element).on("beforeChange", function(event, slick, currentSlide, nextSlide) {
        ".modal__people-with-description" == options.slide && (document.getElementById("block").scrollTop = 0)
    }), $(element).slick(options)
}

function setProgressBarMethods(element, progressBar, progressBarLabel) {
    function getSlidesToShow(settings) {
        var slidesToShow = ["767", "1023"].filter(function(bpoint) {
            if (window.innerWidth < bpoint) return settings[bpoint] && settings[bpoint].slidesToShow
        });
        return (slidesToShow = slidesToShow[0]) && settings[slidesToShow].slidesToShow || settings && settings[1100] && settings[1100].slidesToShow || 1
    }
    element.on("init", function(event, slick, currentSlide) {
        var slidesToShow = getSlidesToShow(slick.breakpointSettings),
            calc = ((slick.currentSlide || 0) + slidesToShow) / slick.$slides.length * 100;
        setProgress(progressBar, progressBarLabel, calc)
    }), element.on("beforeChange", function(event, slick, currentSlide, nextSlide) {
        var calc = (nextSlide + getSlidesToShow(slick.breakpointSettings)) / slick.$slides.length * 100;
        setProgress(progressBar, progressBar, calc)
    })
}

function setProgress(progressBar, progressBarLabel, calc) {
    progressBar.css("background-size", calc + "% 100%").attr("aria-valuenow", calc), progressBarLabel.text(calc + "% completed")
}

function setArrowShowHideMethods(element, leftArrowClass, rightArrowClass) {
    element.on("afterChange", function(event, slick, currentSlide, nextSlide) {
        setArrowVisibilities($(this).parent().find(leftArrowClass), $(this).parent().find(rightArrowClass), currentSlide, slick.slideCount)
    })
}

function setArrowVisibilities(leftArrow, rightArrow, currentSlide, slideCount) {
    0 === currentSlide ? (leftArrow.removeClass("show"), leftArrow.addClass("hide")) : (leftArrow.addClass("show"), leftArrow.removeClass("hide")), currentSlide === slideCount - 1 ? (rightArrow.removeClass("show"), rightArrow.addClass("hide")) : (rightArrow.addClass("show"), rightArrow.removeClass("hide"))
}
$.ajaxPrefilter(function(options, original_Options, jqXHR) {
    options.async = !0
}), window.onscroll = function() {
    lazyLoadImage()
}, document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener("click", function(e) {
        e.preventDefault(), document.querySelector(this.getAttribute("href")).scrollIntoView({
            behavior: "smooth"
        })
    })
});
var myTimeOut, ajax = new XMLHttpRequest;

function bannerTypeWriter(ele, text, ind, speed, callback) {
    for (let i = 0; i < text[ind].length; i++) {
        let textStr = "";
        for (let i = 0; i < ind; i++) textStr += text[i];
        settime(i, speed, textStr)
    }

    function settime(i, speedy, textStr) {
        setTimeout(function() {
            ele.html(textStr + text[ind].substring(0, i + 1) + '<span aria-hidden="true"></span>'), i == text[ind].length - 1 && callback()
        }, speedy * i)
    }
}

function startVariedSpeedAnimationForBanner(ele, parentEle, text, index) {
    parentEle.removeClass("active"), index > text.length - 1 ? parentEle.addClass("active") : bannerTypeWriter(ele, text, index, 2e3 / text[index].length, function() {
        index > text.length || startVariedSpeedAnimationForBanner(ele, parentEle, text, index + 1)
    })
}

function startTextAnimation(ele, i, text) {
    if (0 == startStopAnimation()) $(".home-clients__block .section-title__block").css("min-width", "100%"), $("#first_text").text("Our esteemed customers"), $(".home-clients__titles-block").find(".red__dot").css("margin-left", "0px"), $("#third_text").text(""), $("#second_text").text(""), "Our esteemed customers" == text ? ($("#first_text").text(text), $("#first_text").show()) : "Our valued customers" == text ? ($("#second_text").text(text), $("#second_text").show()) : "Our enthusiastic customers" == text && ($("#third_text").text(text), $("#third_text").show());
    else {
        if (void 0 === text[i]) return void setTimeout(function() {
            startTextAnimation(ele, 0, text)
        }, 5e3);
        i < text[i].length && typeWriter(ele, text, 0, function() {})
    }
}

function detectIEBrowser() {
    return !!window.MSInputMethodContext && !!document.documentMode
}

function startStopAnimation() {
    return !detectIEBrowser()
}

function typeWriter(ele, text, i, fnCallback) {
    i < text.length ? (ele.html(text.substring(0, i + 1) + '<span aria-hidden="true"></span>'), myTimeOut && clearInterval(myTimeOut), myTimeOut = setInterval(function() {
        typeWriter(ele, text, i + 1, fnCallback)
    }, 100)) : "function" == typeof fnCallback && (i = 0, setTimeout(fnCallback, 700))
}
    
var isMenuOpen = !1;

function toggleSideMenu() {
    var $headerContainer = $(".header-common"),
        menubar = $(".header__nav-block");
    $(".header__close-button").removeClass("show");
    $(document.body);
    $headerContainer.toggleClass("show-mobile-menu"), $headerContainer.hasClass("show-mobile-menu") ? (isMenuOpen = !0, disableHTMLScroll(), menubar.addClass("d-show"), $(".header__close-button").addClass("show")) : (isMenuOpen = !1, enableHTMLScroll(), menubar.removeClass("d-show"))
}

function setActiveMenuItem() {
    var urlParts = $(location).attr("href").split("/"),
        pageFound = !1;
    $(".header__menu-dropdown-list").each(function() {
        var attr = $(this).find("a").attr("pagename");
        urlParts[urlParts.length - 2] == attr && (pageFound = !0, $(this).addClass("active").siblings().removeClass("active"))
    }), pageFound || $(".header__menu-dropdown-list").first().addClass("active")
}

function hideHeader() {
    $(".header-common").hide()
}

function showHeader() {
    $(".header-common").show()
}
$(".header__menu-btn").click(function() {
    toggleSideMenu(), lazyLoadImage()
}), $(document).ready(function() {
    $(".header__menu-dropdown-list a").on("click", function() {
        var menuOption = "Selected" + $(this).text();
        $(this).parent().addClass("active").siblings().removeClass("active");
        var pagename = location.pathname.split("/")[1].split(".")[0] + "page Header Menu";
        "page Header Menu" == pagename ? pagename = "home page Header Menu" : $(this).parent().removeClass("active"), google_analytics_tracking(pagename, menuOption, "")
    }), setActiveMenuItem(), $(".footer__icon").attr("src", "assets/images/people_page.svg"), $(".close-button").click(function(e) {
        event.stopPropagation(), toggleSideMenu()
    })
}), $(document).ready(function() {
    var c, currentScrollTop = 0,
        $header = $(".header-common");
    $(window).scrollTop() > 0 && $header.addClass("header-fixed"), $(window).scroll(function() {
        var a = $(window).scrollTop(),
            b = $header.height();
        currentScrollTop = a, a > 0 ? ($(".scrollToTop").fadeIn(), $header.addClass("header-fixed")) : ($(".scrollToTop").fadeOut(), $header.removeClass("header-fixed")), c < currentScrollTop && a > b + b ? isMenuOpen || $header.addClass("scrollUp") : c > currentScrollTop && !(a <= b) && $header.removeClass("scrollUp"), c = currentScrollTop
    });
    var pagenameToAppend = location.pathname.split("/")[1].split(".")[0] + "page Header Menu";
    $(".menu_footer_block .footer-icon").click(function() {
        var socialIcon = $(this).parent().attr("title");
        "" != socialIcon && google_analytics_tracking(pagenameToAppend, socialIcon, "")
    })
}), disableBackgroundScroll("overlay");
var currentImage, imagesData = {
        imageDetails: [{
            linkName: "uxportfolio/transworld",
            imageName: "assets/images/home/avana.jpg",
            webp: "assets/images/home/next-gen/avana.webp",
            altName: "Logistics and Hospitality App UX UI-Avana",
            ariaLabel: "Avana Logistics app designed by Divami",
            title: TAILOR_BLOCK_SECOND_TITLE,
            message: "Whether you are shipping from Bangalore to Kolkata or Australia to the US, there is a good chance your consignment moved through Avana or one of its group logistics companies. As their UX Design Consulting Partner, we are helping Avana through their Digital Transformation journey. We also helped build responsive web apps and mobile apps in Angular, iOS and Android frameworks.",
            mobileMessage: "Channelising logistics management for Avana with our responsive web apps & mobile apps design."
        }, {
            linkName: "uxportfolio/corpus",
            imageName: "assets/images/home/corpus.jpg",
            webp: "assets/images/home/next-gen/corpus.webp",
            altName: "SocialMedia and Entertainment UX UI-Corpus",
            ariaLabel: "Corpus Smart tv platform designed by Divami",
            title: TAILOR_BLOCK_THIRD_TITLE,
            message: "Our UX and UI Designs for the TV and OTT Services helped Corpus build an integrated platform for entertainment and communication that is easy enough for the Indian rural audience, yet appeal to the international market.",
            mobileMessage: "Our UX and UI Designs for the TV and OTT Services helped Corpus build an integrated platform."
        }, {
            linkName: "uxportfolio/semantify",
            imageName: "assets/images/home/semantify.jpg",
            webp: "assets/images/home/next-gen/semantify.webp",
            altName: "Technology Web App UX UI-Semantify",
            ariaLabel: "Semantify analytics platform designed by Divami",
            title: TAILOR_BLOCK_FIRST_TITLE,
            message: "Semantify’s technology platform integrates advanced analytics, natural language and machine learning to derive, in real time, hidden insights from across a wide spectrum of information and data sources. It is a powerful, big data discovery and analytics platform. Our Enterprise UX approach brought simplicity to the platform and helped Semantify scale to a product-centric approach from a service-centric approach. We also helped Semantify build their next generation web UI in Angular.",
            mobileMessage: "We designed the platform integrating advanced analytics, natural language and machine learning to derive, in real time."
        }, {
            linkName: "uxportfolio/incx",
            imageName: "assets/images/home/cryptox.jpg",
            webp: "assets/images/home/next-gen/cryptox.webp",
            altName: "Cryptocurrency Web and Mobile App UX UI-INCX",
            ariaLabel: "Incx Cryptocurrency app designed by Divami",
            title: TAILOR_BLOCK_FOURTH_TITLE,
            message: "Our UX UI Designs for INCX helped revolutionize the Asian Cryptocurrency market with its simplicity and brought cryptocurrency trading to the general public while still catering to expert traders. We also helped develop the trading platform with front-end engineering in React, mobile development for Android and iOS and mid-tier in Node.js.",
            mobileMessage: "Our UX UI Designs for INCX helped revolutionize the Asian Cryptocurrency market & making it simple to use even for the general public."
        }, {
            linkName: "uxportfolio/medicopia",
            imageName: "assets/images/home/medicopia.jpg",
            webp: "assets/images/home/next-gen/medicopia.webp",
            altName: "Healthcare Mobile App UX UI Sample-Medicopia",
            ariaLabel: "Medicopia Healthcare app designed by Divami",
            title: TAILOR_BLOCK_FIFTH_TITLE,
            message: "Our design and development of the Medicopia platform and app helps save lives by helping physicians cut down the prescription errors by bringing comprehensive drug reference within easy reach. The mobile app is available on both iOS and Android app stores.",
            mobileMessage: "With our UX designs we are assisting physicians cut down the prescription errors by bringing comprehensive drug reference within easy reach."
        }, {
            linkName: "uxportfolio/airtel",
            imageName: "assets/images/home/airtel.jpg",
            webp: "assets/images/home/next-gen/airtel.webp",
            altName: "Telecom Mobile App UX UI-Airtel",
            ariaLabel: "Airtel Telecom app designed by Divami",
            title: TAILOR_BLOCK_SIXTH_TITLE,
            message: "We helped Airtel and Robi brands turn their flagship mobile apps in Bangladesh from pure mobile plan management apps to comprehensive consumer services apps complete with digital payments, wallet and ticketing. Our UX UI Designs propelled the apps to 1M+ downloads each.",
            mobileMessage: "Reaching Airtel & Robi consumers of Bangalore by designing multi-services app with digital payments, wallet,  ticketing & other features."
        }, {
            linkName: "uxportfolio/viola",
            imageName: "assets/images/home/viola.jpg",
            webp: "assets/images/home/next-gen/viola.webp",
            altName: "Fintech Mobile App UX UI-Viola",
            ariaLabel: "Viola Fintech app designed by Divami",
            title: TAILOR_BLOCK_SEVENTH_TITLE,
            message: "Viola, a well-established player in the UK, entered the Indian market with Viola wallet - a wallet app that combines several consumer services like bus and movie ticket bookings, payments and even virtual cards. Our UX UI Designs helped establish a unique identity for the Indian market, while continuing to respect their product family identity.",
            mobileMessage: "That’s how we brought UK based e-wallet to India with their diverse services platform like bus and movie ticket bookings."
        }, {
            linkName: "uxportfolio/shiksha",
            imageName: "assets/images/home/shiksha.jpg",
            webp: "assets/images/home/next-gen/shiksha.webp",
            altName: "Education Web App UX UI-Shiksha by HCL",
            ariaLabel: "Shiksha Education app designed by Divami",
            title: TAILOR_BLOCK_EIGTH_TITLE,
            message: "Shiksha Initiative is a technology-driven literacy and enhanced training program of the Shiv Nadar Foundation aimed at eradicating illiteracy. Our UI UX Design and Development work helped build a platform that empowers primary school teachers enhance the learning process with a high-engagement teaching mechanism.",
            mobileMessage: "Designed & developed the platform to empower school teachers to enhance the learning process with engaging teaching mechanisms."
        }]
    },
    verticlePosition = 2;

function setVerticalMessage(position) {
    var $descriptionFit = $(".tailor-fit-description");
    $(window).width() > 1023 ? $descriptionFit.html(imagesData.imageDetails[position].message) : $descriptionFit.html(imagesData.imageDetails[position].mobileMessage)
}

function setTailorFitSectionData(position) {
    $(".home-arrow__nav-image").attr("href", imagesData.imageDetails[position].linkName), $(".home-arrow__nav-image").attr("aria-label", imagesData.imageDetails[position].ariaLabel), $(".home-arrow__image").attr({
        src: imagesData.imageDetails[position].imageName + "?1564635070497",
        alt: imagesData.imageDetails[position].altName
    }), $(".home-arrow__next-gen-image").attr({
        srcset: imagesData.imageDetails[position].webp
    }), $(".tailor-fit-title").html(imagesData.imageDetails[position].title), setVerticalMessage(position)
}

function testimonicalSectionChanges(existingClass, replaceClass, addClass) {
    var $blueBlock = $(".blue__block");
    $blueBlock.removeClass(existingClass), $blueBlock.removeClass(replaceClass), $blueBlock.addClass(addClass)
}

function renderBlogPosts(response) {
    JSON.parse('{"data' + response.split('{"data')[1]).data.forEach(function(blog, index) {
        $("#home-blog-cards").append(blog)
    }), $(".blog__each-post:last-child").append('<div class="blog-view-all"><a target="_blank"  rel="noopener" href="' + BLOG_SERVER_BASE_URL + '">View All</a></div>')
}
$(document).ready(function() {
    $.ajax({
        type: "GET",
        url: BLOG_POSTS_URL,
        data: "",
        success: function(response) {
            renderBlogPosts(response), $(".home-impression__content .loader").hide()
        }
    }), setTailorFitSectionData(verticlePosition), $(".blue__block").addClass("femvista"), $(".home-work__list").on("afterChange", function(event, slick, currentSlide) {
        lazyLoadImage(), 0 == currentSlide ? testimonicalSectionChanges("semantify", "incx", "femvista") : 1 == currentSlide ? testimonicalSectionChanges("femvista", "semantify", "incx") : 2 == currentSlide && testimonicalSectionChanges("incx", "femvista", "semantify")
    }), $(".tailer-section-work-link").hover(function(event) {
        findImage(currentImage = $(this).attr("data-item"))
    }, function(event) {
        currentImage = null
    }), $(".nav__image-block").hover(function(event) {
        currentImage = "continue"
    }, function(event) {
        currentImage = null
    }), startTailorFitAnimation(), $(window).bind("resize", function(e) {
        setVerticalMessage(verticlePosition)
    }), $(".sub-title__block").hover(function(event) {
        currentImage = "continue"
    }, function(event) {
        currentImage = null
    }), $(".home-clients__item").hover(function(event) {
        lazyLoadImage(), $(".home-clients__titles-block").slick("slickPause")
    })
});
var myTimer, classItems = ["Books-&-Track", "Find-Doctor-Review", "Find-Doctor-Review", "Blood", "Refer", "Healthcare", "Pharmacies", "Qualified"],
    currentIndex = 2,
    isTailorSectionIncrement = !0;

function imageMovement(index, image) {
    var flag = "false";
    $(".tailer-section-work-link").parent(".home-arrow__nav-item").removeClass("active"), $(".home-arrow__nav-item:nth-child(" + (index + 1) + ")").addClass("active");
    var itemToHighlight = image;
    if ($(".nav__image-block").removeClass(function(index, classes) {
            return (classes = classes.split(" ")).filter(function(eachClass) {
                return classItems.indexOf(eachClass) > -1
            }).join(" ")
        }).addClass(itemToHighlight), $(window).width() < 768) {
        var totalWidth = $(".home-arrow__nav-block").outerWidth();
        $(".home-arrow__nav-list").css("width", totalWidth);
        var myScrollPos = $(".home-arrow__nav-block .active").offset().left + $(".home-arrow__nav-block .active").outerWidth(!0) / 2 + $(".home-arrow__nav-block .home-arrow__nav-list").scrollLeft() - $(".home-arrow__nav-block .home-arrow__nav-list").width() / 2;
        $(".home-arrow__nav-block .home-arrow__nav-list").animate({
            scrollLeft: myScrollPos
        }, 300), flag = !0
    }
    $(window).on("resize", function() {
        1 == flag && ($(".home-arrow__nav-block").load(location.href + " .home-arrow__nav-block>*", ""), flag = !1)
    })
}

function getTailorImagePosition() {
    isTailorSectionIncrement ? currentIndex >= 7 ? (isTailorSectionIncrement = !1, currentIndex--) : currentIndex++ : 0 == currentIndex ? (isTailorSectionIncrement = !0, currentIndex = 1) : currentIndex--
}

function findImage(imageName) {
    for (var i = 0; i < 8; i++) imageName == classItems[i] && (verticlePosition = i, setTailorFitSectionData(i), imageMovement(i, classItems[i]), currentIndex = i)
}

//function startTailorFitAnimation() {
//    myTimer && clearInterval(myTimer), myTimer = setInterval(function() {
//        currentImage || (getTailorImagePosition(), findImage(classItems[currentIndex]))
//    }, 3e3)
//}

function clientSectionCaurosel() {
    setSlider($(".home-clients__list-block"), {
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: !0,
        infinite: !0,
        autoplaySpeed: 8e3,
        dots: !1,
        adaptiveHeight: !0,
        slide: ".home-clients__list",
        prevArrow: ".clients__left-arrow",
        nextArrow: ".clients__right-arrow",
        asNavFor: ".home-clients__titles-block",
        swipe: !0,
        responsive: [{
            breakpoint: 767,
            settings: {
                autoplay: !0,
                infinite: !0,
                slickGoTo: 1,
                slidesToShow: 1,
                slidesToScroll: 1
            }
        }]
    }, $(".home-clients__list-block .progress__bar"), $(".home-clients__list-block .progress__bar-white"), !1);
    setSliderWithOutNavbars($(".home-clients__titles-block"), {
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: !1,
        swipe: !1,
        autoplay: !0,
        infinite: !0,
        autoplaySpeed: 8e3,
        responsive: [{
            breakpoint: 767,
            settings: {
                slickGoTo: 1,
                slidesToShow: 1,
                slidesToScroll: 1
            }
        }]
    })
}

function navToFilter(ele) {
    let filterType = ele.attr("data-type"),
        filterValue = ele.attr("data-item");
    setLocalStorage(CONST_WORK_FILTER, filterType + "=" + filterValue), ele.attr("href", "uxportfolio")
}
setSlider($(".home-work__list"), {
    slidesToShow: 1,
    slidesToScroll: 1,
    autoplay: !0,
    infinite: !0,
    autoplaySpeed: 6e3,
    dots: !1,
    adaptiveHeight: !0,
    slide: ".home-work__list-item",
    prevArrow: ".work__left-arrow",
    nextArrow: ".work__right-arrow",
    cssEase: "ease-in-out",
    swipe: !0,
    pauseOnHover: !0,
    focusOnSelect: !0,
    responsive: [{
        breakpoint: 767,
        settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            autoplay: !0,
            infinite: !0,
            autoplaySpeed: 2e3,
            slickGoTo: 1
        }
    }]
}, $(".progress__bar__white"), $(".progress__bar-white")), clientSectionCaurosel(), $(document).ready(function() {
    $(".work-nav-link").click(function() {
        $(window).width() > 767 ? navToFilter($(this)) : (startTailorFitAnimation(), findImage(currentImage = $(this).attr("data-item")))
    }), $(".map__icon-link").click(function() {
        navToFilter($(this))
    }), $(".arrow__btn-block").click(function() {
        var $this = $(this),
            navLink = "";
        if ($this.find(".arrow__btn-link-text").text() == READ_MORE) {
            var selectedTailorFit = $this.parent().find(".tailor-fit-title").text(),
                selectedTailorFitVal = $.trim(selectedTailorFit);
            selectedTailorFitVal === TAILOR_BLOCK_FIRST_TITLE ? navLink = "uxportfolio/semantify" : selectedTailorFitVal === TAILOR_BLOCK_SECOND_TITLE ? navLink = "uxportfolio/transworld" : selectedTailorFitVal === TAILOR_BLOCK_THIRD_TITLE ? navLink = "uxportfolio/corpus" : selectedTailorFitVal === TAILOR_BLOCK_FOURTH_TITLE ? navLink = "uxportfolio/incx" : selectedTailorFitVal === TAILOR_BLOCK_FIFTH_TITLE ? navLink = "uxportfolio/medicopia" : selectedTailorFitVal === TAILOR_BLOCK_SIXTH_TITLE ? navLink = "uxportfolio/airtel" : selectedTailorFitVal === TAILOR_BLOCK_SEVENTH_TITLE ? navLink = "uxportfolio/viola" : selectedTailorFitVal === TAILOR_BLOCK_EIGTH_TITLE && (navLink = "uxportfolio/shiksha"),
                function($this, navLink) {
                    $this.find("a").attr("href", "" + navLink)
                }($this, navLink)
        }
    }), $(".home-disrup__card").click(function() {
        var filterValue = $(this).find(".home-disrup__heading").attr("id");
        setLocalStorage(CONST_WORK_FILTER, "focusArea=" + filterValue), $(this).attr("href", "uxportfolio")
    }), $(".trans__center-block").click(function() {
        setLocalStorage(CONST_WORK_FILTER, OUR_SERVICES)
    })
}), $(document).ready(function() {
    $(".arrow__btn-link").click(function() {
        var $section = $(this).find(".arrow__btn-link-text").text();
        if ($section == READ_OUR_CASE_STUDY) google_analytics_tracking("Home page", "Read our case study " + $(this).closest(".work__content-block").find(".work__content-title").text(), "");
        else if ($section == SEE_OUR_PORTFOLIO) google_analytics_tracking("Home page", "See Our Portfolio", "");
        else if ($section == OUR_DESIGN_PROCESS) google_analytics_tracking("Home page", "Our Design Process", "");
        else if ($section == READ_MORE) {
            google_analytics_tracking("Home page", "Read More " + $(this).closest(".section-title__block").find(".tailor-fit-title").text(), "")
        }
    }), $(".clients__right-arrow").click(function() {
        google_analytics_tracking("Home page", "View customers", "")
    }), $(".clients__left-arrow").click(function() {
        google_analytics_tracking("Home page", "View customers", "")
    }), $(".map__icon-link").click(function() {
        var device = $(this).attr("data-item");
        google_analytics_tracking("Home page", device += " Apps in Maps section", "")
    }), $(".nav_item_click").click(function() {
        google_analytics_tracking("Home page", "Tailor-Fit " + $(this).text(), "")
    }), startVariedSpeedAnimationForBanner($(".home-banner__text .home-banner__type-text"), $(".home-banner__text"), ["If you have an Idea..."], 0), startTextAnimation($(".section-title__block .clients__title-text"), 0, "Our esteemed customers"), $(".home-clients__titles-block").on("beforeChange", function(event, slick, currentSlide) {
        $(".section-title__block .clients__title-text").css("opacity", "0"), $(".section-title__block .red__dot").css("opacity", "0")
    }), $(".home-clients__titles-block").on("afterChange", function(event, slick, currentSlide) {
        lazyLoadImage(), $(".section-title__block .clients__title-text").css("opacity", "1"), $(".section-title__block .red__dot").css("opacity", "1");
        var text = "";
        0 == currentSlide ? text = "Our esteemed customers" : 1 == currentSlide ? text = "Our valued customers" : 2 == currentSlide && (text = "Our enthusiastic customers"), startTextAnimation($(".section-title__block .clients__title-text"), 0, text)
    }), setBannerAnimationInitials()
});
var prevScroll, bannerELe, circleBlock, scrollTried = 0,
    animationStarted = reverseAnimationStarted = !1,
    animationCompleted = !1;

function setBannerAnimationInitials() {
    bannerText = $(".home-banner__text-block"), bannerELe = $(".banner__circle-section"), circleBlock = $(".banner__circle-block"), prevScroll = $(window).scrollTop(), $(window).scrollTop() > 150 ? (animationStarted = !0, animationCompleted = !0, showBannerCircle(!1)) : $("body").addClass("banner-position-fixed")
}
var animationTimeout, $window = $(window);

function reverseAnimation() {
    scrollTried = 0, bannerELe.addClass("reverse"), bannerELe.removeClass("completed"), bannerText.removeClass("apply-banner-transitions"), $("body").addClass("banner-position-fixed"), animationTimeout && clearTimeout(animationTimeout), animationTimeout = setTimeout(function() {
        bannerELe.removeClass("active"), circleBlock.addClass("hide"), scrollTried = 0, animationCompleted = !1, reverseAnimationStarted = animationStarted = !1
    }, 700)
}

function showBannerCircle(isFromScroll) {
    circleBlock.removeClass("hide"), bannerELe.addClass("active"), bannerELe.removeClass("reverse"), bannerText.addClass("apply-banner-transitions"), animationTimeout && clearTimeout(animationTimeout), animationTimeout = setTimeout(function() {
        bannerELe.addClass("completed"), scrollTried = 0, isFromScroll && ($window.scrollTop(0), $window.scrollTop(1), $window.scrollTop(2), prevScroll = $window.scrollTop()), animationCompleted = !0, animationStarted = !1, $("body").removeClass("banner-position-fixed")
    }, 700)
}

function validateInputField(element, regularExpression) {
    return $(element).val() != regularExpression
}

function validateEmail() {
    if ("" == $(".email").val()) return !1;
    return !!/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/.test($(".email").val())
}

function doFormValidation() {
    let msg = validateInputField(".form__textarea", ""),
        name = validateInputField(".name", ""),
        email = validateEmail();
    if (!(msg && email && name)) {
        var $errorBox = $(".error_box");
        return $errorBox.addClass("fixed"), $errorBox.hasClass("success") && $errorBox.removeClass("success"), $(".error_message").html(PLEASE_FILL_ALL_THE_FIELDS), $errorBox.show(), setTimeout(function() {
            $errorBox.hide()
        }, 5e3), !1
    }
    doAjaxCall({
        formSelector: $(".contact-form__block")
    })
}

function doAjaxCall(form) {
    var $form = $(form.formSelector),
        data = (form = $form[0], $form.serializeArray());
    doAjaxCallForForm("POST", HUB_SPOT_URL, data, {
        Accept: "application/json",
        "Content-Type": "application/json"
    }, successAlert, tempAlert, function() {
        form.reset()
    })
}

function hideSuccessFailureBlock(msg) {
    $(".error_box").addClass("fixed"), $(".error_message").html(msg), $(".error_box").show(), setTimeout(function() {
        $(".error_box").hide()
    }, 5e3)
}

function tempAlert() {
    $(".error_box").hasClass("success") && $(".error_box").removeClass("success"), hideSuccessFailureBlock(SORRY_TRY_AGAIN)
}

function successAlert() {
    $(".error_box").addClass("success"), hideSuccessFailureBlock(THANK_YOU_FOR_SUBMISSION)
}

function footerTracking(pagename) {
    var footerToAppend = pagename + " page Footer",
        pageToAppend = pagename + " page";
    $(".footer__header-link").click(function() {
        var link = $(this).text();
        google_analytics_tracking(pageToAppend, link, "")
    }), $(".clutch-icon").click(function() {
        google_analytics_tracking(footerToAppend, GA_CLUTCH, "")
    }), $(".footer__facebook-icon").click(function() {
        google_analytics_tracking(footerToAppend, GA_FACEBOOK, "")
    }), $(".footer__linkedin-icon").click(function() {
        google_analytics_tracking(footerToAppend, GA_LINKEDIN, "")
    }), $(".footer__instagram-icon").click(function() {
        google_analytics_tracking(footerToAppend, GA_INSTAGRAM, "")
    }), $(".footer__glassdoor-icon").click(function() {
        google_analytics_tracking(footerToAppend, GA_GLASSDOOR, "")
    }), $(".address__icon").click(function() {
        var country = $(this).closest(".address__block").find(".address__text").text();
        google_analytics_tracking(footerToAppend, country += GA_ADDRESS, "")
    }), $(".form-button__block").click(function() {
        google_analytics_tracking(pageToAppend, GA_FORM_SUBMISSION, "")
    })
}
$window.scroll(function() {
    prevScroll > $window.scrollTop() && $window.scrollTop() < 2 ? reverseAnimationStarted || (reverseAnimationStarted = !0, reverseAnimation(), scrollTried = 0) : animationCompleted || scrollTried++, prevScroll = $window.scrollTop(), scrollTried >= 4 && !animationStarted && (animationStarted = !0, showBannerCircle(!0))
}), $(document).ready(function() {
    $(window).scrollTop(0)
}), $(document).ready(function() {
    let length = location.pathname.split("/").length;
    var pagename = location.pathname.split("/")[length - 1].split(".")[0];
    $(".error_box").hide(), footerTracking(pagename);
    let year = (new Date).getFullYear();
    $(".footer-year").text(year), $(".contact-form__block").submit(function(event) {
        return event.preventDefault(), doFormValidation(), !1
    })
});