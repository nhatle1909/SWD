import React, { useEffect } from 'react';
import {linkImg} from '@/utils/common';
import { useAppDispatch, useAppSelector } from '../../store';
import { actionGetBlogList } from '../../store/blog/action';

export const About = () => {
	const dispatch = useAppDispatch();
    const blogs = useAppSelector(({ blogs }) => blogs.blogs);
    console.log('blogs::', blogs)
    useEffect(() => {
        dispatch(actionGetBlogList({
            pageIndex: 1,
            isAsc: true,
            searchValue: ''
        }))
    }, []);

  	return (
	<>
		<section className="home-slider js-fullheight owl-carousel">
		<div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('about-1.jpg')})` }}>
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

		<div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('about-2.jpg')})` }}>
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
						<div className="img" style={{ backgroundImage: `url(${linkImg('staff-1.jpg')})` }}></div>
						<div className="text pt-4">
							<h3>Lê Nguyên Nhật</h3>
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
						<div className="img" style={{ backgroundImage: `url(${linkImg('staff-2.jpg')})` }}></div>
						<div className="text pt-4">
							<h3>Đoàn Phạm Đăng Khôi</h3>
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
						<div className="img" style={{ backgroundImage: `url(${linkImg('staff-3.jpg')})` }}></div>
						<div className="text pt-4">
							<h3>Bùi Hiểu Khang</h3>
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
						<div className="img" style={{ backgroundImage: `url(${linkImg('staff-4.jpg')})` }}></div>
						<div className="text pt-4">
							<h3>Trần Dương Thảo Uyên</h3>
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
				<div className="col-md-6 col-lg-3 ftco-animate">
					<div className="staff">
						<div className="img" style={{ backgroundImage: `url(${linkImg('staff-4.jpg')})` }}></div>
						<div className="text pt-4">
							<h3>Lê Nguyễn Toàn Năng</h3>
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
					{blogs?.map((item) => (
						<div className='col-md-4' key={item.blogId}>
							<div className="blog-entry">
								<a href="blog-single.html" className="block-20" 
									style={{
										backgroundImage: `url(data:image/jpeg;base64,${item.pictures})`
									}}>
								</a>
								<div className="text d-flex py-4">
									<div className="meta mb-3">
										<div>{item.createdAt.split('T')[0]}</div>
										<div>Staff</div>
										<div className="meta-chat"><span className="icon-chat"></span> 3</div>
									</div>
									<div className="desc pl-3">
										<h3 className="heading"><a href="#">{item.title}</a></h3>
									</div>
								</div>
							</div>
						</div>
					))}
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

export default About;
