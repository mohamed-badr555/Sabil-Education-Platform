// site.js
document.addEventListener('DOMContentLoaded', function() {
  // Mobile sidebar toggle
  const sidebarToggle = document.getElementById('sidebarToggle');
  const sidebar = document.getElementById('sidebar');
  const body = document.body;
  
  if (sidebarToggle && sidebar) {
    sidebarToggle.addEventListener('click', function(event) {
      event.preventDefault();
      event.stopPropagation();
      
      // Toggle sidebar regardless of screen size
      sidebar.classList.toggle('show');
      body.classList.toggle('sidebar-open');
    });
  }
  
  // Close sidebar when clicking outside
  document.addEventListener('click', function(event) {
    if (sidebar && 
        sidebarToggle && 
        !sidebar.contains(event.target) && 
        !sidebarToggle.contains(event.target) && 
        sidebar.classList.contains('show')) {
      sidebar.classList.remove('show');
      body.classList.remove('sidebar-open');
    }
  });
  
  // Active link highlighting
  const currentPath = window.location.pathname;
  const navLinks = document.querySelectorAll('.nav-link');
  
  navLinks.forEach(link => {
    const linkPath = link.getAttribute('href');
    if (linkPath && currentPath.includes(linkPath) && linkPath !== '/') {
      link.classList.add('active');
    } else if (currentPath === '/' && linkPath === '/') {
      link.classList.add('active');
    }
  });
  
  // Initialize tooltips
  if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
    const tooltips = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltips.forEach(tooltip => new bootstrap.Tooltip(tooltip));
  }
  
  // Initialize popovers
  if (typeof bootstrap !== 'undefined' && bootstrap.Popover) {
    const popovers = document.querySelectorAll('[data-bs-toggle="popover"]');
    popovers.forEach(popover => new bootstrap.Popover(popover));
  }

  // Prevent sidebar link clicks from toggling sidebar on mobile
  const sidebarLinks = sidebar ? sidebar.querySelectorAll('a') : [];
  sidebarLinks.forEach(link => {
    link.addEventListener('click', function(event) {
      if (window.innerWidth <= 992) {
        sidebar.classList.remove('show');
        body.classList.remove('sidebar-open');
      }
    });
  });
});
$(document).ready(function () {
    $("#span_chevron").click(function () {
        $("#UnitToggle").slideToggle("");
    });
});
$("#span_chevron").click(function () {
    $("#span_chevron").toggleClass("span");
});

//// Counter animation for stat cards
//function animateCounters() {
//  const counters = document.querySelectorAll('.counter-value');
  
//  counters.forEach(counter => {
//    const target = +counter.getAttribute('data-target');
//    const duration = 1000; // Animation duration in milliseconds
//    const step = target / (duration / 30); // Update every 30ms
//    let current = 0;
    
//    const updateCounter = () => {
//      current += step;
//      if (current >= target) {
//        counter.textContent = target;
//      } else {
//        counter.textContent = Math.floor(current);
//        setTimeout(updateCounter, 30);
//      }
//    };
    
//    updateCounter();
//  });
//}

//// Call animation when page enters viewport
//document.addEventListener('DOMContentLoaded', function() {
//  // Use Intersection Observer to trigger counter animation when stats come into view
//  if ('IntersectionObserver' in window) {
//    const observer = new IntersectionObserver((entries) => {
//      entries.forEach(entry => {
//        if (entry.isIntersecting) {
//          animateCounters();
//          observer.unobserve(entry.target);
//        }
//      });
//    }, { threshold: 0.1 });
    
//    // Observe the first counter element or the container
//    const counterElements = document.querySelectorAll('.counter-value');
//    if (counterElements.length > 0) {
//      observer.observe(counterElements[0].closest('.stat-cards') || counterElements[0]);
//    } else {
//      // Fallback to page load if no counters found
//      animateCounters();
//    }
//  } else {
//    // Fallback for browsers that don't support Intersection Observer
//    window.addEventListener('load', animateCounters);
//  }
//});
