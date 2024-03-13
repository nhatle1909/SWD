const Footer = () => {
    return (
        <footer className="ftco-footer ftco-bg-dark ftco-section">
        <div className="container">
          <div className="row mb-5">
            <div className="col-md">
              <div className="ftco-footer-widget mb-4">
                <h2 className="ftco-heading-2">Klift</h2>
                <p>Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
                <ul className="ftco-footer-social list-unstyled float-md-left float-lft mt-3">
                  <li className="ftco-animate"><a href="#"><span className="icon-twitter"></span></a></li>
                  <li className="ftco-animate"><a href="#"><span className="icon-facebook"></span></a></li>
                  <li className="ftco-animate"><a href="#"><span className="icon-instagram"></span></a></li>
                </ul>
              </div>
            </div>
            <div className="col-md">
              <div className="ftco-footer-widget mb-4 ml-md-4">
                <h2 className="ftco-heading-2">Links</h2>
                <ul className="list-unstyled">
                  <li><a href="#">Home</a></li>
                  <li><a href="#">About</a></li>
                  <li><a href="/blog">Blog</a></li>
                </ul>
              </div>
            </div>
            <div className="col-md">
               <div className="ftco-footer-widget mb-4">
                <h2 className="ftco-heading-2">Services</h2>
                <ul className="list-unstyled">
                  <li><a href="#">Interior Construction Design</a></li>
                </ul>
              </div>
            </div>
            <div className="col-md">
              <div className="ftco-footer-widget mb-4">
                  <h2 className="ftco-heading-2">Have a Questions?</h2>
                  <div className="block-23 mb-3">
                    <ul>
                      <li><span className="icon icon-map-marker"></span><span className="text">Lưu Hữu Phước, Đông Hòa, Dĩ An, Bình Dương, Thành phố Thủ Đức</span></li>
                      <li><a href="#"><span className="icon icon-phone"></span><span className="text">+84 123 456 789</span></a></li>
                      <li><a href="#"><span className="icon icon-envelope"></span><span className="text">info@interiorconstructionquotation.com</span></a></li>
                    </ul>
                  </div>
              </div>
            </div>
          </div>
          <div className="row">
            <div className="col-md-12 text-center">
  
              <p>
    Copyright &copy;<script>document.write(new Date().getFullYear());</script> All rights reserved   <i className="icon-heart" aria-hidden="true"></i>
  </p>
            </div>
          </div>
        </div>
      </footer>
    )
}

export default Footer;