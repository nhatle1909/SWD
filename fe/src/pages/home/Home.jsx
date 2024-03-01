import {linkImg} from '@/utils/common'
const Home = () => {
    return (
        <>
    
    <section className="home-slider js-fullheight owl-carousel">
  <div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('bg_1.jpg')})` }}>
    <div className="overlay"></div>
    <div className="container">
      <div className="row no-gutters slider-text js-fullheight align-items-center justify-content-end" data-scrollax-parent="true">
        <div className="col-md-7 text ftco-animate" data-scrollax=" properties: { translateY: '70%' }">
          <h1 className="mb-4" data-scrollax="properties: { translateY: '30%', opacity: 1.6 }">We Create Amazing Architecture Designs</h1>
          <p data-scrollax="properties: { translateY: '30%', opacity: 1.6 }">A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences fly into your mouth.</p>
          <p><a href="#" className="btn btn-white btn-outline-white px-4 py-3 mt-3">View our works</a></p>
        </div>
      </div>
    </div>
  </div>

  <div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('bg_2.jpg')})` }}>
    <div className="overlay"></div>
    <div className="container">
      <div className="row no-gutters slider-text js-fullheight align-items-center justify-content-end" data-scrollax-parent="true">
        <div className="col-md-7 text ftco-animate" data-scrollax=" properties: { translateY: '70%' }">
          <h1 className="mb-4" data-scrollax="properties: { translateY: '30%', opacity: 1.6 }">Virtually Build Your House</h1>
          <p data-scrollax="properties: { translateY: '30%', opacity: 1.6 }">A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences fly into your mouth.</p>
          <p><a href="#" className="btn btn-white btn-outline-white px-4 py-3 mt-3">View our works</a></p>
        </div>
      </div>
    </div>
  </div>
</section>
		
<section className="ftco-services bg-light">
  <div className="container">
    <div className="row">
      <div className="col-md-4 d-flex align-self-stretch ftco-animate">
        <div className="media block-6 services d-block">
          <div className="icon d-flex justify-content-center align-items-center">
            <span className="flaticon-idea"></span>
          </div>
          <div className="media-body p-2 mt-3">
            <h3 className="heading">Perfectly Design</h3>
            <p>Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic.</p>
          </div>
        </div>
      </div>
      <div className="col-md-4 d-flex align-self-stretch ftco-animate">
        <div className="media block-6 services d-block">
          <div className="icon d-flex justify-content-center align-items-center">
            <span className="flaticon-compass-symbol"></span>
          </div>
          <div className="media-body p-2 mt-3">
            <h3 className="heading">Carefully Planned</h3>
            <p>Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic.</p>
          </div>
        </div>
      </div>
      <div className="col-md-4 d-flex align-self-stretch ftco-animate">
        <div className="media block-6 services d-block">
          <div className="icon d-flex justify-content-center align-items-center">
            <span className="flaticon-layers"></span>
          </div>
          <div className="media-body p-2 mt-3">
            <h3 className="heading">Smartly Execute</h3>
            <p>Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>

		<section className="ftco-section ftc-no-pb">
			<div className="container">
				<div className="row no-gutters">
					<div className="col-md-5 p-md-5 img img-2" style={{backgroundImage: `url(${linkImg('about.jpg')})`}}>
					</div>
					<div className="col-md-7 wrap-about pb-md-5 ftco-animate">
	          <div className="heading-section mb-5 pl-md-5 heading-section-with-line">
	          	<div className="pl-md-5 ml-md-5">
		          	<span className="subheading">About</span>
		            <h2 className="mb-4">We are the best interior &amp; Architect Consultant in Italy</h2>
	            </div>
	          </div>
	          <div className="pl-md-5 ml-md-5 mb-5">
							<p>On her way she met a copy. The copy warned the Little Blind Text, that where it came from it would have been rewritten a thousand times and everything that was left from its origin would be the word "and" and the Little Blind Text should turn around and return to its own, safe country. But nothing the copy said could convince her and so it didn’t take long until a few insidious Copy Writers ambushed her, made her drunk with Longe and Parole and dragged her into their agency, where they abused her for their.</p>
							<p>When she reached the first hills of the Italic Mountains, she had a last view back on the skyline of her hometown Bookmarksgrove, the headline of Alphabet Village and the subline of her own road, the Line Lane. Pityful a rethoric question ran over her cheek, then she continued her way.</p>
							<p><a href="#" className="btn-custom">Learn More <span className="ion-ios-arrow-forward"></span></a></p>
						</div>
					</div>
				</div>
			</div>
		</section>


		<section className="ftco-section ftco-counter img" id="section-counter" style={{ backgroundImage: `url(${linkImg('bg_3.jpg')})` }} data-stellar-background-ratio="0.5">
    	<div className="container">
    		<div className="row d-md-flex align-items-center justify-content-center">
    			<div className="col-lg-4">
    				<div className="heading-section pl-md-5 heading-section-white">
	          	<div className="pl-md-5 ml-md-5 ftco-animate">
		          	<span className="subheading">Some</span>
		            <h2 className="mb-4">Interesting Facts</h2>
	            </div>
	          </div>
    			</div>
    			<div className="col-lg-8">
    				<div className="row d-md-flex align-items-center">
		          <div className="col-md d-flex justify-content-center counter-wrap ftco-animate">
		            <div className="block-18 text-center">
		              <div className="text">
		                <strong className="number" data-number="18">0</strong>
		                <span>Years of Experienced</span>
		              </div>
		            </div>
		          </div>
		          <div className="col-md d-flex justify-content-center counter-wrap ftco-animate">
		            <div className="block-18 text-center">
		              <div className="text">
		                <strong className="number" data-number="351">0</strong>
		                <span>Happy Clients</span>
		              </div>
		            </div>
		          </div>
		          <div className="col-md d-flex justify-content-center counter-wrap ftco-animate">
		            <div className="block-18 text-center">
		              <div className="text">
		                <strong className="number" data-number="564">0</strong>
		                <span>Finished Projects</span>
		              </div>
		            </div>
		          </div>
		          <div className="col-md d-flex justify-content-center counter-wrap ftco-animate">
		            <div className="block-18 text-center">
		              <div className="text">
		                <strong className="number" data-number="300">0</strong>
		                <span>Working Days</span>
		              </div>
		            </div>
		          </div>
	          </div>
          </div>
        </div>
    	</div>
    </section>

    <section className="ftco-section">
    	<div className="container">
    		<div className="row justify-content-center mb-5 pb-2">
          <div className="col-md-7 heading-section ftco-animate">
            <h2 className="mb-4">Our Projects</h2>
            <p>A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences</p>
          </div>
        </div>
    	</div>
    	<div className="container-wrap">
    		<div className="row no-gutters">
    			<div className="col-md-6 col-lg-3 ftco-animate">
    				<div className="project">
	    				<img src="/src/assets/images/work-1.jpg" className="img-fluid" alt="Colorlib Template"/>
	    				<div className="text">
	    					<h3>Office Interior Design in Paris</h3>
	    				</div>
	    				<a href="images/work-1.jpg" className="icon image-popup d-flex justify-content-center align-items-center">
	    					<span className="icon-expand"></span>
	    				</a>
    				</div>
    			</div>
    			<div className="col-md-6 col-lg-3 ftco-animate">
    				<div className="project">
	    				<img src="/src/assets/images/work-2.jpg" className="img-fluid" alt="Colorlib Template"/>
	    				<div className="text">
	    					<h3>Office Interior Design in Paris</h3>
	    				</div>
	    				<a href="images/work-2.jpg" className="icon image-popup d-flex justify-content-center align-items-center">
	    					<span className="icon-expand"></span>
	    				</a>
    				</div>
    			</div>
    			<div className="col-md-6 col-lg-3 ftco-animate">
    				<div className="project">
	    				<img src="/src/assets/images/work-3.jpg" className="img-fluid" alt="Colorlib Template"/>
	    				<div className="text">
	    					<h3>Office Interior Design in Paris</h3>
	    				</div>
	    				<a href="images/work-3.jpg" className="icon image-popup d-flex justify-content-center align-items-center">
	    					<span className="icon-expand"></span>
	    				</a>
    				</div>
    			</div>
    			<div className="col-md-6 col-lg-3 ftco-animate">
    				<div className="project">
	    				<img src={linkImg('work-4.jpg')} className="img-fluid" alt="Colorlib Template"/>
	    				<div className="text">
	    					<h3>Office Interior Design in Paris</h3>
	    				</div>
	    				<a href="images/work-4.jpg" className="icon image-popup d-flex justify-content-center align-items-center">
	    					<span className="icon-expand"></span>
	    				</a>
    				</div>
    			</div>
    		</div>
    	</div>
    </section> 

    <section className="ftco-section testimony-section">
      <div className="container">
        <div className="row justify-content-center mb-5 pb-3">
          <div className="col-md-7 heading-section ftco-animate">
            <h2 className="mb-4">Our satisfied customer says</h2>
            <p>Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in</p>
          </div>
        </div>
        <div className="row ftco-animate">
          <div className="col-md-12">
            <div className="carousel-testimony owl-carousel">
              <div className="item">
                <div className="testimony-wrap p-4 pb-5">
                  <div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_2.jpg')})` }}>
                    <span className="quote d-flex align-items-center justify-content-center">
                      <i className="icon-quote-left"></i>
                    </span>
                  </div>
                  <div className="text">
                    <p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
                    <p className="name">Garreth Smith</p>
                    <span className="position">Marketing Manager</span>
                  </div>
                </div>
              </div>
              <div className="item">
                <div className="testimony-wrap p-4 pb-5">
                  <div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_2.jpg')})` }}>
                    <span className="quote d-flex align-items-center justify-content-center">
                      <i className="icon-quote-left"></i>
                    </span>
                  </div>
                  <div className="text">
                    <p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
                    <p className="name">Garreth Smith</p>
                    <span className="position">Interface Designer</span>
                  </div>
                </div>
              </div>
              <div className="item">
                <div className="testimony-wrap p-4 pb-5">
                  <div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_3.jpg')})` }}>
                    <span className="quote d-flex align-items-center justify-content-center">
                      <i className="icon-quote-left"></i>
                    </span>
                  </div>
                  <div className="text">
                    <p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
                    <p className="name">Garreth Smith</p>
                    <span className="position">UI Designer</span>
                  </div>
                </div>
              </div>
              <div className="item">
                <div className="testimony-wrap p-4 pb-5">
                  <div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_1.jpg')})` }}>
                    <span className="quote d-flex align-items-center justify-content-center">
                      <i className="icon-quote-left"></i>
                    </span>
                  </div>
                  <div className="text">
                    <p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
                    <p className="name">Garreth Smith</p>
                    <span className="position">Web Developer</span>
                  </div>
                </div>
              </div>
              <div className="item">
                <div className="testimony-wrap p-4 pb-5">
                  <div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_3.jpg')})` }}>
                    <span className="quote d-flex align-items-center justify-content-center">
                      <i className="icon-quote-left"></i>
                    </span>
                  </div>
                  <div className="text">
                    <p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
                    <p className="name">Garreth Smith</p>
                    <span className="position">System Analyst</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
			
		<section className="ftco-section">
			<div className="container">
				<div className="row justify-content-center mb-5 pb-3">
          <div className="col-md-7 heading-section ftco-animate">
            <h2 className="mb-4">Our Architect Team</h2>
            <p>Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in</p>
          </div>
        </div>	
				<div className="row">
					<div className="col-md-6 col-lg-3 ftco-animate">
						<div className="staff">
							<div className="img" style={{ backgroundImage: `url(${linkImg('staff_1.jpg')})` }}></div>
							<div className="text pt-4">
								<h3>David Smith</h3>
								<span className="position mb-2">Achitect</span>
								<p>I am an ambitious workaholic, but apart from that, pretty simple person.</p>
								<ul className="ftco-social d-flex">
	                <li className="ftco-animate"><a href="#"><span className="icon-twitter"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-facebook"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-google-plus"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-instagram"></span></a></li>
	              </ul>
							</div>
						</div>
					</div>
					<div className="col-md-6 col-lg-3 ftco-animate">
						<div className="staff">
							<div className="img" style={{ backgroundImage: `url(${linkImg('staff_2.jpg')})` }}></div>
							<div className="text pt-4">
								<h3>David Smith</h3>
								<span className="position mb-2">Achitect</span>
								<p>I am an ambitious workaholic, but apart from that, pretty simple person.</p>
								<ul className="ftco-social d-flex">
	                <li className="ftco-animate"><a href="#"><span className="icon-twitter"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-facebook"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-google-plus"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-instagram"></span></a></li>
	              </ul>
							</div>
						</div>
					</div>
					<div className="col-md-6 col-lg-3 ftco-animate">
						<div className="staff">
							<div className="img" style={{ backgroundImage: `url(${linkImg('staff_3.jpg')})` }}></div>
							<div className="text pt-4">
								<h3>David Smith</h3>
								<span className="position mb-2">Achitect</span>
								<p>I am an ambitious workaholic, but apart from that, pretty simple person.</p>
								<ul className="ftco-social d-flex">
	                <li className="ftco-animate"><a href="#"><span className="icon-twitter"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-facebook"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-google-plus"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-instagram"></span></a></li>
	              </ul>
							</div>
						</div>
					</div>
					<div className="col-md-6 col-lg-3 ftco-animate">
						<div className="staff">
							<div className="img" style={{ backgroundImage: `url(${linkImg('staff_4.jpg')})` }}></div>
							<div className="text pt-4">
								<h3>David Smith</h3>
								<span className="position mb-2">Achitect</span>
								<p>I am an ambitious workaholic, but apart from that, pretty simple person.</p>
								<ul className="ftco-social d-flex">
	                <li className="ftco-animate"><a href="#"><span className="icon-twitter"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-facebook"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-google-plus"></span></a></li>
	                <li className="ftco-animate"><a href="#"><span className="icon-instagram"></span></a></li>
	              </ul>
							</div>
						</div>
					</div>
				</div>
			</div>
		</section>

		<section className="ftco-section">
			<div className="container">
				<div className="row justify-content-center mb-5 pb-3">
          <div className="col-md-7 heading-section ftco-animate">
            <h2 className="mb-4">Recent Blog</h2>
            <p>Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in</p>
          </div>
        </div>	
				<div className="row">
          <div className="col-md-4 ftco-animate">
            <div className="blog-entry">
              <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_2.jpg')})` }}>
              </a>
              <div className="text d-flex py-4">
                <div className="meta mb-3">
                  <div><a href="#">Sep. 20, 2018</a></div>
                  <div><a href="#">Admin</a></div>
                  <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                </div>
                <div className="desc pl-3">
	                <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
	              </div>
              </div>
            </div>
          </div>
          <div className="col-md-4 ftco-animate">
            <div className="blog-entry" data-aos-delay="100">
              <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_2.jpg')})` }}>
              </a>
              <div className="text d-flex py-4">
                <div className="meta mb-3">
                  <div><a href="#">Sep. 20, 2018</a></div>
                  <div><a href="#">Admin</a></div>
                  <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                </div>
                <div className="desc pl-3">
	                <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
	              </div>
              </div>
            </div>
          </div>
          <div className="col-md-4 ftco-animate">
            <div className="blog-entry" data-aos-delay="200">
              <a href="blog-single.html" className="block-20" style={{ backgroundImage: `url(${linkImg('image_3.jpg')})` }}>
              </a>
              <div className="text d-flex py-4">
                <div className="meta mb-3">
                  <div><a href="#">Sep. 20, 2018</a></div>
                  <div><a href="#">Admin</a></div>
                  <div><a href="#" className="meta-chat"><span className="icon-chat"></span> 3</a></div>
                </div>
                <div className="desc pl-3">
	                <h3 className="heading"><a href="#">Even the all-powerful Pointing has no control about the blind texts</a></h3>
	              </div>
              </div>
            </div>
          </div>
        </div>
			</div>
		</section>

		<section className="ftco-section ftc-no-pb">
			<div className="container">
				<div className="row no-gutters">
					<div className="col-md-5 p-md-5 img img-2 order-md-last" style={{ backgroundImage: `url(${linkImg('img.jpg')})` }}>
					</div>
					<div className="col-md-7 wrap-about pb-md-5 ftco-animate">
	          <div className="heading-section mb-md-5 pl-md-5 heading-section-with-line">
	          	<div className="pr-md-5 mr-md-5">
		          	<span className="subheading">Perfect</span>
		            <h2 className="mb-4">We Make Perfection</h2>
	            </div>
	          </div>
	          <div className="pr-md-5 mr-md-5">
							<p>On her way she met a copy. The copy warned the Little Blind Text, that where it came from it would have been rewritten a thousand times and everything that was left from its origin would be the word "and" and the Little Blind Text should turn around and return to its own, safe country. But nothing the copy said could convince her and so it didn’t take long until a few insidious Copy Writers ambushed her, made her drunk with Longe and Parole and dragged her into their agency, where they abused her for their.</p>
							<p>When she reached the first hills of the Italic Mountains, she had a last view back on the skyline of her hometown Bookmarksgrove, the headline of Alphabet Village and the subline of her own road, the Line Lane. Pityful a rethoric question ran over her cheek, then she continued her way.</p>
							<p><a href="#" className="btn-custom">Learn More <span className="ion-ios-arrow-forward"></span></a></p>
						</div>
					</div>
				</div>
			</div>
		</section>
        </>
    )
}

export default Home;